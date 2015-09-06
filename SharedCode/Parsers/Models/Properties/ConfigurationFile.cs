#region

using System;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.Properties
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class ConfigurationFile
    {
        private String _path;
        [XmlAttribute("Path")]
        public String Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
