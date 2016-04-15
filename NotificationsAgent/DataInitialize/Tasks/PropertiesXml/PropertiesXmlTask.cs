using SharedCode.IO;
using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NotificationsAgent.DataInitialize.Tasks.PropertiesXml
{
    /// <summary>
    /// Zarządza deserializacją pliku properties.
    /// </summary>
    public class PropertiesXmlTask : ITask<DataInitializeTaskOutput>
    {
        /// <summary>
        /// Wywołuje cały proces.
        /// </summary>
        /// <param name="input">Dane wejściowe</param>
        /// <param name="output">Obiekt wypełniany w czasie wywołania</param>
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            PropertiesXmlTaskInput propertiesXmlTaskInput = (PropertiesXmlTaskInput)input;

            String configurationFileName = propertiesXmlTaskInput.PropertiesFileName;

            ScheduledAgent.IoManager.GetFileStreamFromStorageFolder(
                configurationFileName,
                StreamType.ForWrite,
                out output.propertiesFile);

            try
            {
                ScheduledAgent.XmlParser.Deserialize(
                    ScheduledAgent.PropertiesFileName,
                    out output.propertiesXmlResult);
            }
            catch (Exception e)
            {
                output.AddException(e);
            }
        }
    }
}
