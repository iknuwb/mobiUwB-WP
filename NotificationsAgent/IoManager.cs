using SharedCode.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using Windows.Storage;

namespace NotificationsAgent
{
    /// <summary>
    /// Zarządza systemem plików
    /// </summary>
    public class IoManager : IIOManagment
    {
        /// <summary>
        /// Sprawdza czy plik o podanej nazwie istnieje w domyślnym folderze.
        /// </summary>
        /// <param name="fileName">Nazwa szukanego pliku</param>
        /// <returns>Informacja czy plik istnieje</returns>
        public Boolean CheckIfFileExists(String fileName)
        {
            String folder = ApplicationData.Current.LocalFolder.Path;
            String fullSavePath = Path.Combine(
                folder,
                fileName);
            return File.Exists(fullSavePath);
        }

        /// <summary>
        /// Tworzy plik o podanej nazwie w domyślnym folderze oraz otwiera strumień do tego pliku.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Strumień do pliku</returns>
        public Stream CreateNewFileInStorageFolder(
            String fileName)
        {
            fileName = Path.GetFileName(fileName);
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            Task<StorageFile> file = storageFolder.CreateFileAsync(
                fileName,
                CreationCollisionOption.OpenIfExists).AsTask();
            file.Wait();

            Task<Stream> fileStream = file.Result.OpenStreamForWriteAsync();
            fileStream.Wait();
            return fileStream.Result;
        }

        /// <summary>
        /// Czyta zawartość tekstową pliku.
        /// </summary>
        /// <param name="file">Ścieżka do pliku</param>
        /// <returns>Treść pliku</returns>
        public String ReadResourceFileData(Uri file)
        {
            StreamResourceInfo strm = Application.GetResourceStream(
                file);
            String fileContents;
            using (StreamReader reader = new StreamReader(strm.Stream))
            {
                fileContents = reader.ReadToEnd();
            }
            return fileContents;
        }

        /// <summary>
        /// Czyta treść pliku o podanej nazwie z domyślnegu folderu.
        /// </summary>
        /// <param name="fileName">Nazwa pliku</param>
        /// <returns>Treśc pliku</returns>
        public String ReadLocalFileData(String fileName)
        {
            String fullSavePath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                fileName);
            String fileContents;

            using (StreamReader reader = new StreamReader(fullSavePath))
            {
                fileContents = reader.ReadToEnd();
            }

            return fileContents;
        }

        /// <summary>
        /// Pobiera strumień pliku z domyślnego folderu.
        /// </summary>
        /// <param name="fileName">Nazwa pliku</param>
        /// <param name="streamType">Typ strumienia</param>
        /// <param name="stream">Uzyskany strumień</param>
        public void GetFileStreamFromStorageFolder(
            String fileName,
            StreamType streamType,
            out Stream stream)
        {
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            Task<StorageFile> fileTask =
                storageFolder.GetFileAsync(fileName).AsTask();
            fileTask.Wait();
            Task<Stream> streamTask;
            switch (streamType)
            {
                case StreamType.ForRead:
                    {
                        streamTask =
                            fileTask.Result.OpenStreamForReadAsync();
                        break;
                    }
                case StreamType.ForWrite:
                    {
                        streamTask =
                            fileTask.Result.OpenStreamForWriteAsync();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(
                            "Non existing enum value. " +
                            typeof(StreamType));
                    }
            }
            streamTask.Wait();
            stream = streamTask.Result;
        }

        /// <summary>
        /// Pobiera plik z internetu do domyślnego folderu.
        /// </summary>
        /// <param name="uriToDownload">Ścieżka do pliku</param>
        /// <param name="destinationFileName">Nazwa pliku do zapisania.</param>
        /// <param name="cToken">Informacja czy operacja powinna być anulowana</param>
        /// <returns>Wątek zwracający wynik po zakończeniu</returns>
        public async Task<DownloadResult> DownloadFileFromWebToStorageFolder(Uri uriToDownload, string destinationFileName, CancellationToken cToken)
        {
            DownloadResult result = new DownloadResult();
            try
            {
                using (Stream mystr = await DownloadFile(uriToDownload))
                {
                    using (Stream stream =
                        CreateNewFileInStorageFolder(
                            destinationFileName))
                    {
                        const int bufferSize = 1024;
                        byte[] buf = new byte[bufferSize];

                        int bytesread = 0;
                        while ((bytesread = await mystr.ReadAsync(buf, 0, bufferSize)) > 0)
                        {
                            cToken.ThrowIfCancellationRequested();
                            stream.Write(buf, 0, bytesread);
                        }
                    }
                }
                result.AddException(DownloadInfo.Succeded);
                return result;
            }
            catch (OperationCanceledException exception)
            {
                result.AddException(DownloadInfo.Cancelled, exception);
                return result;
            }
            catch (Exception exception)
            {
                result.AddException(DownloadInfo.Other, exception);
                return result;
            }
        }

        /// <summary>
        /// Pobiera plik z internetu do domyślnego folderu.
        /// </summary>
        /// <param name="internetFilePath">Ścieżka do pliku</param>
        /// <param name="destinationFileName">Nazwa pliku do zapisania.</param>
        /// <param name="cToken">Informacja czy operacja powinna być anulowana</param>
        /// <returns>Wątek zwracający wynik po zakończeniu</returns>
        public async Task<DownloadResult> DownloadFileFromWebToStorageFolder(string internetFilePath, string destinationFileName, CancellationToken cToken)
        {
            Uri dowloadFileUri = new Uri(internetFilePath);
            return await DownloadFileFromWebToStorageFolder(
                dowloadFileUri,
                destinationFileName,
                cToken);
        }

        /// <summary>
        /// Otwiera strumień do pliku internetowego.
        /// </summary>
        /// <param name="url">Ścieżka do pliku</param>
        /// <returns>Wątek zawierający strumień do pliku po zakończeniu</returns>
        private Task<Stream> DownloadFile(Uri url)
        {
            var tcs = new TaskCompletionSource<Stream>();
            var wc = new WebClient();
            wc.OpenReadCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.TrySetException(e.Error);
                }
                else if (e.Cancelled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(e.Result);
                }
            };
            wc.OpenReadAsync(url);
            return tcs.Task;
        }

        /// <summary>
        /// Kopjuje plik z assets do domyślnego folderu.
        /// </summary>
        /// <param name="filePath">Relatywna ścieżka do pliku</param>
        /// <returns>Wątek</returns>
        public async Task CopyFileFromAssetsToStorageFolder(String filePath)
        {
            String assetsPath = Path.Combine("Assets", filePath);
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            String fileName = Path.GetFileName(filePath);

            StreamResourceInfo streamResourceInfo =
                Application.GetResourceStream(new Uri(assetsPath, UriKind.Relative));
            using (Stream resourceFileStream = streamResourceInfo.Stream)
            {
                StorageFile newFile = await storageFolder.CreateFileAsync(fileName);
                using (Stream newFileStream = await newFile.OpenStreamForWriteAsync())
                {
                    resourceFileStream.CopyTo(newFileStream);
                }
            }
        }
    }
}
