using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent.DataInitialize.Tasks.ConfigurationXml
{
    /// <summary>
    /// Zarządza deserializacją pliku configuration.
    /// </summary>
    public class ConfigurationXmlTask : ITask<DataInitializeTaskOutput>
    {
        /// <summary>
        /// Wywołuje cały proces.
        /// </summary>
        /// <param name="input">Dane wejściowe</param>
        /// <param name="output">Obiekt wypełniany w czasie wywołania</param>
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            try
            {
                ScheduledAgent.XmlParser.Deserialize(
                    ScheduledAgent.ConfigurationFileName,
                    out output.configXmlResult);
            }
            catch (Exception e)
            {
                output.AddException(e);
            }
        }
    }
}
