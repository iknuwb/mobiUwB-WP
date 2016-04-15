#region

using System;
using SharedCode.DataManagment.DataAccess;

#endregion

namespace SharedCode.DataManagment
{
    /// <summary>
    /// Zarządza zapisywaniem i odczytywaniem odpowiednich plików.
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// Umożliwia czytanie plików.
        /// </summary>
        private readonly IReadData _readData;
        /// <summary>
        /// Umożliwia zapisywanie plików.
        /// </summary>
        private readonly IWriteData _writeData;

        public DataManager(IReadData readData, IWriteData writeData)
        {
            _readData = readData;
            _writeData = writeData;
        }

        /// <summary>
        /// Odczytuje dane o podanym kluczu.
        /// </summary>
        /// <typeparam name="T">Typ odczytywanych danych</typeparam>
        /// <param name="key">Klucz</param>
        /// <returns>Odczytane dane</returns>
        public T RestoreData<T>(String key) where
            T : IRestolable<T>, new()
        {
            lock (this)
            {
                T obj;
                _readData.Read(out obj, key);
                return obj;
            }
        }


        /// <summary>
        /// Zapisuje dane pod podanym kluczu.
        /// </summary>
        /// <typeparam name="T">Typ zapisywanych danych</typeparam>
        /// <param name="key">Klucz</param>
        public void StoreData<T>(T templateModel, String key) where
            T : IRestolable<T>, new()
        {
            lock (this)
            {
                _writeData.Write(templateModel, key);
            }
        }
    }
}
