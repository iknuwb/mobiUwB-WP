using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.IO
{
    /// <summary>
    /// Model obiektu wynikowego pobierania.
    /// </summary>
    public class DownloadResult : IOResult
    {
        private DownloadInfo _information;
        /// <summary>
        /// Pobiera informację o wyniku pobierania.
        /// </summary>
        public DownloadInfo Information
        {
            get { return _information; }
            private set { _information = value; }
        }

        /// <summary>
        /// Umożliwia dodanie wyjątku oraz nadanie wyniku pobierania.
        /// </summary>
        /// <param name="information">Wynik pobierania</param>
        /// <param name="exception">Wyjątek</param>
        public void AddException(DownloadInfo information, Exception exception = null)
        {
            if (exception != null)
            {
                AddException(exception);
            }
            Information = information;
        }
    }
}
