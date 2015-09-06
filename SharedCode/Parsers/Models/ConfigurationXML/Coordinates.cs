#region

using System;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class Coordinates
    {
        private Double _lattitude;
        [XmlElement("szerokosc")]
        public Double Lattitude
        {
            get { return _lattitude; }
            set { _lattitude = value; }
        }

        private Double _longtitude;
        [XmlElement("dlugosc")]
        public Double Longtitude
        {
            get { return _longtitude; }
            set { _longtitude = value; }
        }
    }
}
