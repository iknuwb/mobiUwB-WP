#region

using System;

#endregion

namespace SharedCode.DataManagment.DataAccess
{
    /// <summary>
    /// Interfejs do zapisywania odpowiednich danych.
    /// </summary>
    public interface IWriteData
    {
        void Write<T>(T obj, String key) where
            T : IRestolable<T>, new();
    }
}
