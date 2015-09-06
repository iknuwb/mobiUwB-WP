#region

using System;
using System.Collections.Generic;
using System.IO;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models.Properties;
using SharedCode.Tasks.Models;
using SharedCode.VersionControl.Models;

#endregion

namespace NotificationsAgent.DataInitialize
{
    /// <summary>
    /// Reprezentuje model inicjalizacyjny usługi.
    /// </summary>
    public class DataInitializeTaskOutput : TaskOutput
    {
        /// <summary>
        /// Strumień do pliku properties.
        /// </summary>
        public Stream propertiesFile;
        /// <summary>
        /// Strumień do pliku configuration.
        /// </summary>
        public Stream configurationFile;
        /// <summary>
        /// Zdeserializowany plik properties.
        /// </summary>
        public Properties propertiesXmlResult;
        /// <summary>
        /// Wynik VersionController'a.
        /// </summary>
        public VersioningResult versioningResult;
        /// <summary>
        /// Zdeserializowany plik configuration.
        /// </summary>
        public Configuration configXmlResult;
        /// <summary>
        /// Informuje czy powiadomienia są włączone.
        /// </summary>
        public bool isNotificationActive;
        /// <summary>
        /// Informuje czy zakres czasowy jest włączony.
        /// </summary>
        public bool isTimeRangeActive;
        /// <summary>
        /// ZWartość zakresu godzinowego od.
        /// </summary>
        public DateTime timeRangeFrom;
        /// <summary>
        /// ZWartość zakresu godzinowego do.
        /// </summary>
        public DateTime timeRangeTo;
        /// <summary>
        /// Index wybranego odstępu czasowego.
        /// </summary>
        public int intervalIndex;
        /// <summary>
        /// Wartość wybranego odstępu czasowego.
        /// </summary>
        public long interval;
        /// <summary>
        /// ID obecnie wybranego wydziału.
        /// </summary>
        public String CurrentUnitId;
        /// <summary>
        /// Słownik wszystkich wartości wybranych w ekranie ustawień.
        /// </summary>
        public Dictionary<String, object> allValues;
        /// <summary>
        /// Słownik informując które kategorie są zaznaczone.
        /// </summary>
        public Dictionary<String, Boolean> categories;

        /// <summary>
        /// Wypełnia wartości domyślne.
        /// </summary>
        public DataInitializeTaskOutput()
        {
            allValues = new Dictionary<String, object>();
            categories = new Dictionary<String, Boolean>();
        }
    }
}
