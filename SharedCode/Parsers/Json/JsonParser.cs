using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using SharedCode.Parsers.Json.Model;

namespace SharedCode.Parsers.Json
{
    /// <summary>
    /// Zarządza parsowaniem JSON-a.
    /// </summary>
    public class JsonParser
    {
        /// <summary>
        /// Parsuje plik zawierający informacje typu Feed.
        /// </summary>
        /// <param name="jsonContent">Treść pliku zawierającego JSON</param>
        /// <returns>Lista uzyskanych informacji</returns>
        public List<Feed> ParseFeedsJson(String jsonContent)
        {
            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(List<Feed>));

            List<Feed> jsonRoot;
            using (Stream jsonStream = GenerateStreamFromString(jsonContent))
            {
                jsonRoot = (List<Feed>)serializer.ReadObject(jsonStream);
            }
            return jsonRoot;
        }

        /// <summary>
        /// Tworzy strumień z tekstu.
        /// </summary>
        /// <param name="s">Tekst, z którego tworzymy strumień.</param>
        /// <returns></returns>
        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
