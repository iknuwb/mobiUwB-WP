using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using SharedCode.VersionControl.Models;
using SharedCode.VersionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VersionControllerClass = SharedCode.VersionControl.VersionController;
using SharedCode.IO;


namespace NotificationsAgent.DataInitialize.Tasks.VersionController
{
    /// <summary>
    /// Zarządza kontrolą aktualności pliku.
    /// </summary>
    public class VersionControllerTask : ITask<DataInitializeTaskOutput>
    {
        /// <summary>
        /// Wywołuje cały proces.
        /// </summary>
        /// <param name="input">Dane wejściowe</param>
        /// <param name="output">Obiekt wypełniany w czasie wywołania</param>
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            VersionControllerTaskInput versionControllerTaskInput = (VersionControllerTaskInput)input;
            String configurationFileName = ScheduledAgent.ConfigurationFileName;

            VersioningRequest versioningRequest = new VersioningRequest(
                    output.propertiesXmlResult.Configuration.Path,
                    configurationFileName);

            VersionControllerClass versionController = 
                new VersionControllerClass(
                    ScheduledAgent.IoManager, 
                    ScheduledAgent.ReadData, 
                    ScheduledAgent.WriteData);

            Task<VersioningResult> versioningResultTask = versionController.GetNewestFile(versioningRequest);
            VersioningResult versioningResult = versioningResultTask.Result;

            output.AddExceptions(versioningResult.GetExceptions());

            if (output.Succeeded)
            {
                ScheduledAgent.IoManager.GetFileStreamFromStorageFolder(
                    configurationFileName,
                    StreamType.ForWrite,
                    out output.configurationFile);
            }

            output.versioningResult = versioningResult;
        }
    }
}
