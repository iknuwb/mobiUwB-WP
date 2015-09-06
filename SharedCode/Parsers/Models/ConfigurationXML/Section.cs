#region

using System;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class Section
    {
        private bool _sectionNotifications;
        [XmlAttribute("powiadomienia")]
        public bool SectionNotifications
        {
            get { return _sectionNotifications; }
            set { _sectionNotifications = value; }
        }

        private String _sectionId;
        [XmlElement("id_sekcji")]
        public String SectionId
        {
            get { return _sectionId; }
            set { _sectionId = value; }
        }
        

        private String _sectionTitle;
        [XmlElement("tytul_sekcji")]
        public String SectionTitle
        {
            get { return _sectionTitle; }
            set { _sectionTitle = value; }
        }
    }
}
