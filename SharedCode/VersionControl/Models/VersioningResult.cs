#region



#endregion

#region

using System;

#endregion

namespace SharedCode.VersionControl.Models
{
    /// <summary>
    /// Model wyjściowy VersionControllera.
    /// </summary>
    public class VersioningResult : Result
    {
        private VersioningRequest _versioningRequest;
        /// <summary>
        /// Pobiera obiekt modelu wejściowego.
        /// </summary>
        /// <returns>Obiekt modelu wejściowego</returns>
        public VersioningRequest GetVersioningRequest()
        {
            return _versioningRequest;
        }
        /// <summary>
        /// Nadaje model obiektu wejściowego
        /// </summary>
        /// <param name="versioningRequest">Model obiektu wejściowego</param>
        public void SetVersioningRequest(
                VersioningRequest versioningRequest)
        {
            _versioningRequest = versioningRequest;
        }

        private String _fileContent;
        /// <summary>
        /// Pobiera treść pliku.
        /// </summary>
        /// <returns>Treść pliku</returns>
        public String GetFileContent()
        {
            return _fileContent;
        }
        /// <summary>
        /// Nadaje treść pliku.
        /// </summary>
        /// <param name="fileContent">Treść pliku</param>
        public void SetFileContent(String fileContent)
        {
            _fileContent = fileContent;
        }
    }
}
