using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode
{
    /// <summary>
    /// Przechowuje wartości domyślne
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// ID template'a powiadomień.
        /// </summary>
        public const String NotificationsActiveId = "notifications";
        /// <summary>
        /// ID template'a częstotliwości.
        /// </summary>
        public const String FrequencyId = "frequency";
        /// <summary>
        /// ID template'a zakresu godzin.
        /// </summary>
        public const String TimeRangeId = "timeRange";
        /// <summary>
        /// ID template'a zakresu od.
        /// </summary>
        public const String FromId = "from";
        /// <summary>
        /// ID template'a zakresu do.
        /// </summary>
        public const String ToId = "to";
        /// <summary>
        /// Domyślna wartość template'a zakresu od.
        /// </summary>
        public static readonly DateTime FromDefaultValue = 
            new DateTime(
                DateTime.Now.Year, 
                DateTime.Now.Month, 
                DateTime.Now.Day, 
                6, 
                0,
                0);
        /// <summary>
        /// Domyślna wartość template'a zakresu dp.
        /// </summary>
        public static readonly DateTime ToDefaultValue =
            new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                18,
                0,
                0);
        /// <summary>
        /// Domyślny index template'a częstotliwości.
        /// </summary>
        public const int DefaultFrequencyIndex = 0;
        /// <summary>
        /// Klucz obecnie wybranej jednostki.
        /// </summary>
        public const String currentUnitId = "currentUnitId";
        /// <summary>
        /// Domyślna wartość zakresu godzin.
        /// </summary>
        public const bool TimeRangeDefaultValue = true;
        /// <summary>
        /// Domyślna wartości częstotliwości.
        /// </summary>
        public static readonly List<int> Frequencies = 
            new List<int>
            {
                60 * 1000, 
                10 * 60 * 1000, 
                3600 * 1000, 
                2 * 3600 * 1000, 
                6 * 3600 * 1000, 
                12 * 3600 * 1000, 
                24 * 3600 * 1000
            };
    }
}
