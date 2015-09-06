#region

using System;

#endregion

namespace SharedCode.DataManagment.DataAccess
{
    /// <summary>
    /// Interfejs do wczytywania odpowiednich danych.
    /// </summary>
    public interface IReadData
    {
        void Read<T>(out T obj, String key) where
            T : IRestolable<T>, new();
    }
}
