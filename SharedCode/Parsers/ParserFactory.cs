#region

using System;
using System.Collections.Generic;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace SharedCode.Parsers
{
    /// <summary>
    /// Zbiór metod pomocniczych.
    /// </summary>
    public static class ParserFactory
    {
        /// <summary>
        /// Tworzy listę instytutów na podstawie listy Website oraz obrazka.
        /// </summary>
        /// <param name="websites">Lista stron</param>
        /// <param name="imagePath">Ścieżka do obrazka</param>
        /// <returns>Lista modeli instytutu</returns>
        public static List<InstitiuteModel> GenerateInstituteModels(
            List<Website> websites, String imagePath)
        {
            List<InstitiuteModel> modelsList = new List<InstitiuteModel>();
            foreach (var website in websites)
            {
                modelsList.Add(GenerateInstituteModel(website, imagePath));
            }
            return modelsList;
        }

        /// <summary>
        /// Tworzy listę instytutów na podstawie opakowywacza stron oraz obrazka.
        /// </summary>
        /// <param name="websites">Opakowywacz stron</param>
        /// <param name="imagePath">Ścieżka do obrazka</param>
        /// <returns>Lista modeli instytutu</returns>
        public static List<InstitiuteModel> GenerateInstituteModels(
            Websites websites, String imagePath)
        {
            return GenerateInstituteModels(websites.WebsiteList, imagePath);
        }

        /// <summary>
        /// Tworzy jeden model instytutu na podstawie strony i obrazka.
        /// </summary>
        /// <param name="website">Strona</param>
        /// <param name="imagePath">Ścieżka do obrazka</param>
        /// <returns>Utworzony instytut</returns>
        public static InstitiuteModel GenerateInstituteModel(
            Website website, String imagePath)
        {
            return new InstitiuteModel(website, imagePath);
        }

        /// <summary>
        /// Wyszukuje modelu instytutu na podstawie strony.
        /// </summary>
        /// <param name="website">Strona</param>
        /// <param name="wrappers">modele instytutów</param>
        /// <returns>Znaleziony instytut</returns>
        public static InstitiuteModel FindWrapperBy(
            Website website, 
            List<InstitiuteModel> wrappers)
        {
            foreach (var wrapper in wrappers)
            {
                if (wrapper.Website.Id.Equals(website.Id))
                {
                    return wrapper;
                }
            }
            return null;
        }
    }
}
