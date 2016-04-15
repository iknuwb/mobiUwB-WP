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
    [XmlRoot("konfiguracja", Namespace = "http://ii.uwb.edu.pl/serwis/konfiguracja/czesc2")]
    public class Configuration
    {
        private Tutors _tutors;
        [XmlElement("opiekunowie")]
        public Tutors Tutors
        {
            get { return _tutors; }
            set { _tutors = value; }
        }

        private Authors _authors;
        [XmlElement("autorzy")]
        public Authors Authors
        {
            get { return _authors; }
            set { _authors = value; }
        }

        private List<Unit> _units;

        [XmlElement("jednostka")]
        public List<Unit> Units
        {
            get { return _units; }
            set { _units = value; }
        }


        public Unit GetUnitById(String id)
        {
            foreach (Unit unit in _units)
            {
                if (unit.Id.Equals(id))
                {
                    return unit;
                }
            }
            return null;
        }
    }
}
