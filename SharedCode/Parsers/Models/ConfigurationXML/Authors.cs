#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace SharedCode.Parsers.Models.ConfigurationXML
{
    /// <summary>
    /// Model odwzorowujący dane z pliku XML.
    /// </summary>
    public class Authors
    {
        private List<String> _authorsList;
        [XmlElement("autor")]
        public List<String> AuthorsList
        {
            get { return _authorsList; }
            set { _authorsList = value; }
        }

    }
}
