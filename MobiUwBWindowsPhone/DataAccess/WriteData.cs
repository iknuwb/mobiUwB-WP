#region

using System;
using System.IO.IsolatedStorage;
using SharedCode.DataManagment;
using SharedCode.DataManagment.DataAccess;

#endregion

namespace MobiUwB.DataAccess
{
    /// <summary>
    /// Umożliwia zapisanie danych
    /// </summary>
    public class WriteData : IWriteData
    {
        /// <summary>
        /// Zapisuje dane
        /// </summary>
        /// <typeparam name="T">Typ zapisywanych danych</typeparam>
        /// <param name="obj">Dane do zapisania</param>
        /// <param name="key">Klucz pod jakim zapisać dane</param>
        public void Write<T>(T obj, String key) where
            T : IRestolable<T>, new()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (!settings.Contains(key))
            {
                settings.Add(key, obj);
            }
            else
            {
                settings[key] = obj;
            }
            settings.Save();
        }
    }
}
