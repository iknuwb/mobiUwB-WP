using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using SharedCode.Utilities;

namespace NotificationsAgent.DataInitialize.Tasks.UnitIdFinder
{
    /// <summary>
    /// Zarządza bezpiecznym wątkowo deserializatorem pliku.
    /// </summary>
    public class UnitIdFinderTask : ITask<DataInitializeTaskOutput>
    {
        /// <summary>
        /// Wywołuje cały proces.
        /// </summary>
        /// <param name="input">Dane wejściowe</param>
        /// <param name="output">Obiekt wypełniany w czasie wywołania</param>
        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            using (Mutex mutex = new Mutex(true, Variables.UnitIdMutexName))
            {
                mutex.WaitOne();
                try
                {
                    IsolatedStorageFile isolatedStorageFile =
                        IsolatedStorageFile.GetUserStoreForApplication();
                    if (isolatedStorageFile.FileExists(Variables.UnitIdFileName))
                    {
                        using (IsolatedStorageFileStream isolatedStorageFileStream = new
                            IsolatedStorageFileStream(
                            Variables.UnitIdFileName,
                            FileMode.Open,
                            isolatedStorageFile))
                        {
                            DataContractJsonSerializer dataContractJsonSerializer =
                                new DataContractJsonSerializer(
                                    typeof(UnitIdWrapper));

                            UnitIdWrapper unitIdWrapper =
                                (UnitIdWrapper)dataContractJsonSerializer.ReadObject(
                                    isolatedStorageFileStream);

                            output.CurrentUnitId = unitIdWrapper.UnitId;
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException(
                            "File containing current unit id not found.");
                    }
                }
                catch (Exception e)
                {
                    output.AddException(e);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}
