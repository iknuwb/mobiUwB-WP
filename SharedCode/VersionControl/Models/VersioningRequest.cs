#region

using System;

#endregion

namespace SharedCode.VersionControl.Models
{
    /// <summary>
    /// Model wejściowy VersionControllera.
    /// </summary>
    public class VersioningRequest
    {
        private readonly String _internetFilePath;
        /// <summary>
        /// Pobiera ścieżkę do pliku do pobrania.
        /// </summary>
        /// <returns>Ścieżka do pliku</returns>
        public String GetInternetFile()
        {
            return _internetFilePath;
        }

        private readonly String _destinationFileName;
        /// <summary>
        /// Pobiera docelową nazwę pliku.
        /// </summary>
        /// <returns>Docelowa nazwa pliku</returns>
        public String GetDestinationFileName()
        {
            return _destinationFileName;
        }

        public VersioningRequest(String internetFilePath, String destinationFileName)
        {
            _internetFilePath = internetFilePath;
            _destinationFileName = destinationFileName;
        }
    }
}
