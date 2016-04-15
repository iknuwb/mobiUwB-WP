#region

using System;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class Unit
    {
        private static readonly String DotSignConvention = "(kropka)";
        private static readonly String EmailSignConvenction = "(malpa)";

        private String _id;
        [XmlAttribute("xml:id")]
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;
        [XmlElement("nazwa")]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _fullTitle;
        [XmlElement("pelny_tytul")]
        public String MyProperty
        {
            get { return _fullTitle; }
            set { _fullTitle = value; }
        }

        private String _logo;
        [XmlElement("logo")]
        public String Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        private Sections _sections;
        [XmlElement("sekcje")]
        public Sections Sections
        {
            get { return _sections; }
            set { _sections = value; }
        }

        private Map _map;
        [XmlElement("mapa")]
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        private Address _address;
        [XmlElement("adres")]
        public Address Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private String _email;
        [XmlElement("email")]
        public String Email
        {
            get { return _email; }
            set 
            {
                _email = BeautifyEmail(value); 
            }
        }

        private String _phone1;
        [XmlElement("tel1")]
        public String Phone1
        {
            get { return _phone1; }
            set { _phone1 = value; }
        }

        private String _phone2;
        [XmlElement("tel2")]
        public String Phone2
        {
            get { return _phone2; }
            set { _phone2 = value; }
        }

        private String _fax;
        [XmlElement("fax")]
        public String Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }


        private String _apiUrlString;
        [XmlElement("apiUrl")]
        public String ApiUrlString
        {
            get { return _apiUrlString; }
            set { _apiUrlString = value; }
        }


        private String BeautifyEmail(String uglyEmail)
        {
            return uglyEmail.
                Replace(
                    DotSignConvention, ".").
                Replace(
                    EmailSignConvenction, "@");
        }
    }
}
