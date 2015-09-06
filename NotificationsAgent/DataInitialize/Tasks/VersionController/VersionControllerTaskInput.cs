using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent.DataInitialize.Tasks.VersionController
{
    /// <summary>
    /// Model klasy VersionControllerTask.
    /// </summary>
    public class VersionControllerTaskInput : TaskInput
    {
        private String _configurationFileName;
        /// <summary>
        /// Pobiera lub nadaje nazwę pliku konfiguracyjnego.
        /// </summary>
        public String ConfigurationFileName
        {
            get { return _configurationFileName; }
            set { _configurationFileName = value; }
        }

        public VersionControllerTaskInput(String configurationFileName)
        {
            this._configurationFileName = configurationFileName;
        }
    }
}
