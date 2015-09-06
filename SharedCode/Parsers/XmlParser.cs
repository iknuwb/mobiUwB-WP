#region

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using SharedCode.IO;

#endregion

namespace SharedCode.Parsers
{
    /// <summary>
    /// Zarządza parsowaniem plików XML.
    /// </summary>
    public class XmlParser
    {
        /// <summary>
        /// Zapewnia dostęp do systemu plików.
        /// </summary>
        private readonly IIOManagment _ioManager;

        public XmlParser(IIOManagment ioManager)
        {
            _ioManager = ioManager;
        }

        /// <summary>
        /// Deserializuje plik XML.
        /// </summary>
        /// <typeparam name="T">Typ elementu głównego</typeparam>
        /// <param name="xmlFilePName">Nazwa pliku</param>
        /// <param name="rootElement">Zwracany obiekt po zakończeniu funkcji</param>
        public void Deserialize<T>(
            String xmlFilePName,
            out T rootElement)
        {
            String fileName = Path.GetFileName(xmlFilePName);

            Stream stream;
            _ioManager.GetFileStreamFromStorageFolder(fileName,StreamType.ForRead, out stream);
            using (stream)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                rootElement = (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Serializuje plik XML.
        /// </summary>
        /// <typeparam name="T">Typ elementu głównego</typeparam>
        /// <param name="rootElement">Główny element</param>
        /// <param name="fileName">Nazwa pliku pod jaką serializujemy</param>
        public void Serialize<T>(T rootElement, String fileName)
        {
            Stream stream = null;
            try
            {
                if (_ioManager.CheckIfFileExists(fileName) == false)
                {
                    _ioManager.CreateNewFileInStorageFolder(fileName);
                }

                _ioManager.GetFileStreamFromStorageFolder(
                    fileName,
                    StreamType.ForWrite,
                    out stream);

                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(
                    stream,
                    rootElement);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }
    }
}
