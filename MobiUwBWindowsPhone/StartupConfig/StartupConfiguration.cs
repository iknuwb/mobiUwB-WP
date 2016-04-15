#region

using System;
using System.ComponentModel;
using MobiUwB.StartupConfig.Worker;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace MobiUwB.StartupConfig
{
    /// <summary>
    /// Opakowywacz klasy StartupBackgroundWorker.
    /// </summary>
    public class StartupConfiguration
    {
        /// <summary>
        /// Przechowuje nazwę wewnętrznego pliku konfiguracyjnego.
        /// </summary>
        public static String PropertiesFileName = "properties.xml";
        /// <summary>
        /// Przechowuje nazwę zewnętrznego pliku konfiguracyjnego.
        /// </summary>
        public static String ConfigurationFileName = "configuration.xml";
        /// <summary>
        /// Przechowuje zdeserializowny plik properties.
        /// </summary>
        public static Properties Properties;
        /// <summary>
        /// Przechowuje zdeserializowany plik configuration.
        /// </summary>
        public static Configuration Configuration;
        /// <summary>
        /// Występuje gdy konfiguracja dobiegła końca.
        /// </summary>
        public event RunWorkerCompletedEventHandler Finished;
        /// <summary>
        /// Obiekt klasy wykonującej cały proces inicjalizacji poczatkowych danych.
        /// </summary>
        private readonly StartupBackgroundWorker _startupBackgroundWorker;

        /// <summary>
        /// Inicjalizuje niezbędne zmienne.
        /// </summary>
        public StartupConfiguration()
        {
            _startupBackgroundWorker = new StartupBackgroundWorker();
            _startupBackgroundWorker.RunWorkerCompleted += OnRunWorkerCompleted; 
        }

        /// <summary>
        /// Rozpoczyna działanie wątku zbierającego dane.
        /// </summary>
        public void startConfiguration()
        {
            _startupBackgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Występuje gdy proces dobiegnie końca.
        /// </summary>
        /// <param name="sender">Obiekt wywołujący</param>
        /// <param name="workerCompletedEventArgs">Zawiera parametry przesłane do metody</param>
        private void OnRunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs workerCompletedEventArgs)
        {
            StartupConfigurationResult startupConfigurationResult =
                (StartupConfigurationResult) workerCompletedEventArgs.Result;

            Properties = startupConfigurationResult.Properties;
            Configuration = startupConfigurationResult.Configuration;

            RunWorkerCompletedEventHandler handler = Finished;
            if (handler != null)
            {
                handler(this, workerCompletedEventArgs);
            }
        }
    }
}
