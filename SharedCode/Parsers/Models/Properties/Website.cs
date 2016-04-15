#region

using System;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.Properties
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class Website
    {
        private String _id;
        [XmlAttribute("ID")]
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlAttribute("Name")]
        public String Name { get; set; }

        private String _url;
        [XmlAttribute("URL")]
        public String Url
        {
            get { return _url; }
            set 
            {
                _url = value;
            }
        }

        private String _ping;
        [XmlAttribute("Ping")]
        public String Ping
        {
            get { return _ping; }
            set 
            {
                _ping = value;
            }
        }

        public override int GetHashCode()
        {
            return 4;
        }

        public override bool Equals(object obj)
        {
            if (obj is Website)
            {
                Website that = obj as Website;
                if (that.Url == Url)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
