using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Parsers.Json.Model
{
    /// <summary>
    /// Model pojedyńczej nowej informacji wykorzystywany w parserze.
    /// </summary>
    [DataContract]
    public class Feed
    {
        private String _title;
        /// <summary>
        /// Pobiera lub nadaje tytuł informacji.
        /// </summary>
        [DataMember(Name="tytul")]
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }


        private String _content;
        /// <summary>
        /// Pobiera lub nadaje treść informacji.
        /// </summary>
        [DataMember(Name="tresc")]
        public String Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// Pobiera lub nadaje czas publikacji informacji.
        /// </summary>
        private DateTime _dateTime;
        [DataMember(Name = "data")]
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }
    }
}
