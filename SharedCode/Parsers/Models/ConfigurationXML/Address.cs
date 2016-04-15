#region

using System;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class Address
    {
        private String _postalCode;
        [XmlElement("kod")]
        public String PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        private String _city;
        [XmlElement("miasto")]
        public String City 
        {
            get { return _city; }
            set { _city = value; }
        }

        private String _street;
        [XmlElement("ulica")]
        public String Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private String _number;
        [XmlElement("numer")]
        public String Number
        {
            get { return _number; }
            set { _number = value; }
        }

    }
}
