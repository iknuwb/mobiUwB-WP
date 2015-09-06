using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode.Utilities
{
    /// <summary>
    /// Zmienne globalne na potrzeby całej solucji.
    /// </summary>
    public static class Variables
    {
        /// <summary>
        /// Nazwa mutexa blokującego dostęp do pliku.
        /// </summary>
        public const String UnitIdMutexName = "UnitId";
        /// <summary>
        /// Nazwa pliku zawierającego ID obecnie wybranego wydziału.
        /// </summary>
        public const String UnitIdFileName = "Unit.json";
    }
}
