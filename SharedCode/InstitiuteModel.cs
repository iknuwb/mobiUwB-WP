#region

using System;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace SharedCode
{
    /// <summary>
    /// Model instytutu
    /// </summary>
    public class InstitiuteModel
    {
        /// <summary>
        /// Obiekt Website na podstawie którego powstał.
        /// </summary>
        public readonly Website Website;

        /// <summary>
        /// Pobiera nazwę instytutu.
        /// </summary>
        public String Name
        {
            get
            {
                return Website.Name;
            }
        }
        
        /// <summary>
        /// Pobiera adres instytutu.
        /// </summary>
        public String Page 
        { 
            get 
            {
                return Website.Url;
            }
        }

        /// <summary>
        /// Pobiera ścieżkę do pliku.
        /// </summary>
        public String IconPath { get; private set; }

        public InstitiuteModel(Website website, 
            String iconPath)
        {
            Website = website;
            IconPath = iconPath;
        }

        /// <summary>
        /// Porównuje ten obiekt do innego.
        /// </summary>
        /// <param name="obj">Obiekt do którego porównujemy</param>
        /// <returns>Wynik porównania</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is InstitiuteModel))
            {
                return false;
            }
            InstitiuteModel concreteObj = (InstitiuteModel) obj;
            return concreteObj.Website.Id.Equals(Website.Id);
        }

        /// <summary>
        /// Zwraca HashCode obiektu.
        /// </summary>
        /// <returns>HashCode obiektu</returns>
        public override int GetHashCode()
        {
            return 0;
        }
    }
}
