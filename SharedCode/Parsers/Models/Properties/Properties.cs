#region

using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.Properties
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    [XmlRoot("Properties")]
    public class Properties
    {
        private ConfigurationFile _configuration;
        [XmlElement("ConfigurationFile")]
        public ConfigurationFile Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        private Websites _websites;
        [XmlElement("Websites")]
        public Websites Websites
        {
            get { return _websites; }
            set { _websites = value; }
        }

    }
}
