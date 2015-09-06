using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Utilities
{
    /// <summary>
    /// Opakowywacz wartości settings
    /// </summary>
    public class SettingsCategoriesValuesWrapper
    {
        private Dictionary<String, object> _settingsCategoriesValues;
        /// <summary>
        /// Pobiera lub nadaje słownik wartości.
        /// </summary>
        public Dictionary<String, object> SettingsCategoriesValues
        {
            get { return _settingsCategoriesValues; }
            set { _settingsCategoriesValues = value; }
        }

        /// <summary>
        /// Dodaje wartość do słownika.
        /// </summary>
        /// <param name="key">Klucz wartości</param>
        /// <param name="value">Wartość</param>
        public void AddValue(String key, object value)
        {
            _settingsCategoriesValues.Add(key, value);
        }

        /// <summary>
        /// Pobiera wartość słownika.
        /// </summary>
        /// <param name="key">Klucz elementu słownika</param>
        /// <returns>Wartość ze słownika o podanym kluczu</returns>
        public object GetValue(String key)
        {
            return _settingsCategoriesValues[key];
        }

        /// <summary>
        /// Inicjalizuje wartości domyślne.
        /// </summary>
        public SettingsCategoriesValuesWrapper()
        {
            _settingsCategoriesValues = new Dictionary<String, object>();
        }
    }
}
