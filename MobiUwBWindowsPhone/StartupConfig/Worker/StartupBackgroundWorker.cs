#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using MobiUwB.DataAccess;
using MobiUwB.Utilities;
using SharedCode.VersionControl;
using SharedCode.VersionControl.Models;
using SharedCode.Parsers;
using MobiUwB.IO;

#endregion

namespace MobiUwB.StartupConfig.Worker
{
    /// <summary>
    /// Zajmuje się przygotowaniem danych niezbędnych do uruchomienia aplikacji.
    /// </summary>
    public class StartupBackgroundWorker : BackgroundWorker
    {
        /// <summary>
        /// Przechowuje wynik działania tej klasy
        /// </summary>
        private StartupConfigurationResult _result;
        /// <summary>
        /// Przechowuje ścieżkę do pliku properties.xml wewnątrz katalogu domyślnego.
        /// </summary>
        private String _readWriteXmlPath;
        /// <summary>
        /// Przechowuje ścieżkę do pliku config.xml wewnątrz katalogu domyślnego.
        /// </summary>
        private String _configurationXmlPath;

        /// <summary>
        /// Inicjuje niezbędne zdarzenia.
        /// </summary>
        public StartupBackgroundWorker()
        {
            SetEvents();
        }

        /// <summary>
        /// Inicjuje niezbędne zdarzenia.
        /// </summary>
        private void SetEvents()
        {
            DoWork += StartupBackgroundWorker_DoWork;
        }

        /// <summary>
        /// Wywołuje się na start wątku.
        /// </summary>
        /// <param name="e">Zawiera parametry przesłane do metody</param>
        private void OnStartupBackgroundWorker_DoWorkStart(DoWorkEventArgs e)
        {
            _result = new StartupConfigurationResult();
            e.Result = _result;
        }

        /// <summary>
        /// Wykonuje kolejkę zbierania wszystkich danych.
        /// </summary>
        /// <param name="sender">Obiekt wywołujący</param>
        /// <param name="e">Zawiera parametry przesłane do metody</param>
        void StartupBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OnStartupBackgroundWorker_DoWorkStart(e);
            try
            {
                PrepareFilePaths();
                Task propTask = DeserializePropertiesXml(_result);
                propTask.Wait();
                Task configTask = DeserializeConfigurationXml(_result);
                configTask.Wait();
                StoreCurrentUnitId(_result);
            }
            catch (Exception exception)
            {
                _result.AddException(exception);
            }
            OnStartupBackgroundWorker_DoWorkEnd(e);
        }
        /// <summary>
        /// Wykonuje się jako ostatni etap działania wątku.
        /// </summary>
        /// <param name="e"></param>
        private void OnStartupBackgroundWorker_DoWorkEnd(DoWorkEventArgs e)
        {
            _result.PrintExceptions();
        }

        /// <summary>
        /// Zapisuje zmienną do pliku aby usługa mogła z niej zkorzystać.
        /// </summary>
        /// <param name="result">Informacje zebrane przez tą klasę</param>
        private void StoreCurrentUnitId(StartupConfigurationResult result)
        {
            UnitIdStorer unitIdStorer = new UnitIdStorer();
            Globals.CurrentUnitId = result.Properties.Websites.DefaultWebsite.Id;
            unitIdStorer.RunWorkerAsync(
                Globals.CurrentUnitId);
        }

        /// <summary>
        /// Pobiera i deserializuje plik config.xml
        /// </summary>
        /// <param name="result">Obiekt zbierający dane wynikowe, do którego przypisana zostanie wartość.</param>
        /// <returns>Wątek</returns>
        private async Task DeserializeConfigurationXml(StartupConfigurationResult result)
        {
            ReadData readData = new ReadData();
            WriteData writeData = new WriteData();

            VersionController versionController = 
                new VersionController(
                    Globals.IoManager, 
                    readData, 
                    writeData);

            VersioningRequest versioningRequest =
                new VersioningRequest(
                    result.Properties.Configuration.Path, StartupConfiguration.ConfigurationFileName);

            VersioningResult versioningResult =
                await versionController.GetNewestFile(versioningRequest);

            if (versioningResult.Succeeded)
            {
                App.XmlParser.Deserialize(_configurationXmlPath, out result.Configuration);
            }
            else
            {
                result.AddExceptions(versioningResult.GetExceptions());
            }
        }

        /// <summary>
        /// Tworzy pełne ścieżki do plików konfiguracyjnych.
        /// </summary>
        private void PrepareFilePaths()
        {
            _readWriteXmlPath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path, StartupConfiguration.PropertiesFileName);


            _configurationXmlPath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path, StartupConfiguration.ConfigurationFileName);
        }

        /// <summary>
        /// Deserializuje plik properties.xml.
        /// </summary>
        /// <param name="result">Obiekt zbierający dane wynikowe, do którego przypisana zostanie wartość.</param>
        /// <returns>Wątek</returns>
        private async Task DeserializePropertiesXml(StartupConfigurationResult result)
        {
            String fileName = StartupConfiguration.PropertiesFileName;
            if (!Globals.IoManager.CheckIfFileExists(_readWriteXmlPath))
            {
                await Globals.IoManager.CopyFileFromAssetsToStorageFolder(fileName);
            }
            App.XmlParser.Deserialize(fileName,
                out result.Properties);
        }
    }
}
