#region

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SharedCode.DataManagment;
using SharedCode.DataManagment.DataAccess;
using SharedCode.IO;
using SharedCode.VersionControl.Models;

#endregion

namespace SharedCode.VersionControl
{
    /// <summary>
    /// Zarządza aktualnością pliku.
    /// </summary>
    public class VersionController
    {
        /// <summary>
        /// Zapewnia dostęp do systemu plików.
        /// </summary>
        private readonly IIOManagment _ioManager;

        /// <summary>
        /// Zapewnia dostęp do czytania danych.
        /// </summary>
        private readonly IReadData _readData;

        /// <summary>
        /// Zapewnia dostęp do zapisu danych.
        /// </summary>
        private readonly IWriteData _writeData;

        public VersionController(
            IIOManagment ioManager, 
            IReadData readData, 
            IWriteData writeData)
        {
            _ioManager = ioManager;
            _readData = readData;
            _writeData = writeData;
        }

        /// <summary>
        /// Metoda główna pobierająca najnowszy plik.
        /// </summary>
        /// <param name="versioningRequest">Obiekt wejściowy</param>
        /// <returns>Wątek zawierający obiekt wyjściowy po zakończeniu działania.</returns>
        public async Task<VersioningResult> GetNewestFile(VersioningRequest versioningRequest)
        {
            VersioningResult result = new VersioningResult();
            try
            {
                Boolean deviceFileExists =
                    _ioManager.CheckIfFileExists(
                        versioningRequest.GetDestinationFileName());

                if (deviceFileExists)
                {
                    Boolean isDeviceFileCurrent = await CheckIfFileIsCurrent(versioningRequest);
                    if (!isDeviceFileCurrent)
                    {
                        await DownloadFile(versioningRequest);
                    }
                }
                else
                {
                    await DownloadFile(versioningRequest);
                }

                FillResultWithLocalFile(
                    versioningRequest,
                    result);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return result;
        }

        /// <summary>
        /// Pobiera plik.
        /// </summary>
        /// <param name="request">Dane wejściowe.</param>
        /// <returns>Wątek</returns>
        private async Task DownloadFile(VersioningRequest request)
        {
            DownloadResult downloadInfo = await _ioManager.DownloadFileFromWebToStorageFolder(
                request.GetInternetFile(),
                request.GetDestinationFileName(),
                CancellationToken.None);

            if (!downloadInfo.Succeeded)
            {
                throw downloadInfo.GetExceptions().FirstOrDefault();
            }
        }

        /// <summary>
        /// Sprawdza czy plik jest aktualny.
        /// </summary>
        /// <param name="request">Dane wejściowe</param>
        /// <returns>Czy plik jest aktualny</returns>
        private async Task<Boolean> CheckIfFileIsCurrent(VersioningRequest request)
        {
            DataManager dataManager = new DataManager(_readData, _writeData);
            FileModificationDate fileModificationDate =
                dataManager.RestoreData<FileModificationDate>(
                    request.GetInternetFile());

            DateTime internetFileDateTime = await GetInternetFileModificationDate(request.GetInternetFile());
            if (fileModificationDate.Date < internetFileDateTime)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Pobiera datę modyfikacji pliku internetowego
        /// </summary>
        /// <param name="internetFile">Ścieżka do pliku</param>
        /// <returns>Wątek z datą modyfikacji po zakończeniu</returns>
        public async Task<DateTime> GetInternetFileModificationDate(String internetFile)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(internetFile);

            DateTime dt;
            using (WebResponse response =
                await Task<WebResponse>.Factory.FromAsync(
                req.BeginGetResponse,
                req.EndGetResponse,
                req))
            {
                String lastModifiedHeaderContent = response.Headers["Last-Modified"];
                if (String.IsNullOrEmpty(lastModifiedHeaderContent))
                {
                    return DateTime.Now;
                }
                dt = DateTime.Parse(lastModifiedHeaderContent);
            }
            return dt;
        }

        /// <summary>
        /// Wypełnia obiekt wynikowy treścią pliku.
        /// </summary>
        /// <param name="request">Obiekt wejściowy</param>
        /// <param name="result">Obiekt wyjściowy</param>
        private void FillResultWithLocalFile(
            VersioningRequest request,
            VersioningResult result)
        {
            result.SetFileContent(
                _ioManager.ReadLocalFileData(
                request.GetDestinationFileName()));
        }

        /// <summary>
        /// Opakowywacz daty modyfikacji.
        /// </summary>
        private class FileModificationDate : IRestolable<FileModificationDate>
        {
            private DateTime _date;
            /// <summary>
            /// Pobiera lub nadaje datę modyfikacji.
            /// </summary>
            public DateTime Date
            {
                get { return _date; }
                set { _date = value; }
            }

            public FileModificationDate()
            {
                _date = DateTime.MinValue;
            }

            /// <summary>
            /// Pobiera wartość domyślną.
            /// </summary>
            /// <returns>Wartość domyślna</returns>
            public FileModificationDate GetDefaults()
            {
                return new FileModificationDate();
            }
        }
    }
}
