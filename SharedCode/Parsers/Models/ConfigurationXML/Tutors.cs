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
    public class Tutors
    {
        private List<String> _tutorsList;
        [XmlElement("opiekun")]
        public List<String> TutorsList
        {
            get { return _tutorsList; }
            set { _tutorsList = value; }
        }

    }
}
