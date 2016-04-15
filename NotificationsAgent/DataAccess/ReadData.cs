#region

using System;
using System.IO.IsolatedStorage;
using SharedCode.DataManagment;
using SharedCode.DataManagment.DataAccess;

#endregion

namespace NotificationsAgent.DataAccess
{
    /// <summary>
    /// Umożliwia odzyskanie wcześniej zapisanych danych
    /// </summary>
    public class ReadData : IReadData
    {
        /// <summary>
        /// Odtwarza zapisane dane
        /// </summary>
        /// <typeparam name="T">Typ zapisanych danych implementujących IRestolable</typeparam>
        /// <param name="obj">Odzyskane dane</param>
        /// <param name="key">Klucz pod jakim zapisano dane</param>
        public void Read<T>(out T obj, String key) where 
            T : IRestolable<T>, new()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains(key))
            {
                obj = (T)settings[key];
            }
            else
            {
                obj = new T().GetDefaults();
            }
        }
    }
}
