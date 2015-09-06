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
    public class Sections
    {
        private List<Section> _sectionsList;
        [XmlElement("sekcja")]
        public List<Section> SectionsList
        {
            get { return _sectionsList; }
            set { _sectionsList = value; }
        }

        private List<StaticSection> _staticSectionsList;

        public List<StaticSection> StaticSectionList
        {
            get { return _staticSectionsList; }
            set { _staticSectionsList = value; }
        }


        public Section GetSectionById(String id)
        {
            Section result = null;
            foreach (Section item in _sectionsList)
            {
                if (item.SectionId == id)
                {
                    return item;
                }
            }
            return result;
        }
    }
}
