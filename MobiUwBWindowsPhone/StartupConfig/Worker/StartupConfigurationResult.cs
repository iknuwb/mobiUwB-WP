#region

using SharedCode;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace MobiUwB.StartupConfig.Worker
{
    /// <summary>
    /// Model klasy StartupBacgroundWorker.
    /// </summary>
    public class StartupConfigurationResult : Result
    {
        /// <summary>
        /// Przechowuje zdeserializowany plik properties.xml.
        /// </summary>
        public Properties Properties;
        /// <summary>
        /// Przechowuje zdeserializowany plik configuration.xml.
        /// </summary>
        public Configuration Configuration;
    }
}
