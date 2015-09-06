#region

using System;
using Windows.ApplicationModel.Store;
using MobiUwB.IO;

#endregion

namespace MobiUwB.Utilities
{
    /// <summary>
    /// Przechowuje zmienne globalne.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Dostęp do menadżera plików.
        /// </summary>
        public static readonly IoManager IoManager = new IoManager();

        private static String _currentUnitId = "";
        /// <summary>
        /// Dostęp do id wybranego wydziału.
        /// </summary>
        public static string CurrentUnitId
        {
            get
            {
                lock (_currentUnitId)
                {
                    return _currentUnitId;
                }
            }
            set
            {
                lock (_currentUnitId)
                {
                    _currentUnitId = value;
                }
            }
        }
    }
}
