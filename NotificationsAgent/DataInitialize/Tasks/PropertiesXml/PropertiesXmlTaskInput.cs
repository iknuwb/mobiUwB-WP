using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent.DataInitialize.Tasks.PropertiesXml
{
    /// <summary>
    /// Model wejściowy klasy PropertiesXmlTask.
    /// </summary>
    public class PropertiesXmlTaskInput : TaskInput
    {
        private String _propertiesFileName;
        /// <summary>
        /// Nadaje lub pobiera nazwę pliku.
        /// </summary>
        public String PropertiesFileName
        {
            get { return _propertiesFileName; }
            set { _propertiesFileName = value; }
        }

        public PropertiesXmlTaskInput(String propertiesFileName)
        {
            this._propertiesFileName = propertiesFileName;
        }
    }
}
