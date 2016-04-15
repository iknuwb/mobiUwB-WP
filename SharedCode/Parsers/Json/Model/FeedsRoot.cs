using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Parsers.Json.Model
{
    /// <summary>
    /// Model opakowujący.
    /// </summary>
    [DataContract]
    public class FeedsRoot
    {
        private List<Feed> _feeds;
        /// <summary>
        /// Pobiera lub nadaje listę informacji.
        /// </summary>
        [DataMember]
        public List<Feed> Feeds
        {
            get { return _feeds; }
            set { _feeds = value; }
        }

        /// <summary>
        /// Inicjalizuje wartości domyślne.
        /// </summary>
        public FeedsRoot()
        {
            _feeds = new List<Feed>();
        }
    }
}
