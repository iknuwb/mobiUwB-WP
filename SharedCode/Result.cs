#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#endregion

namespace SharedCode
{
    /// <summary>
    /// Bazowy model wyniku.
    /// </summary>
    public abstract class Result
    {
        /// <summary>
        /// Lista wyjątków.
        /// </summary>
        private readonly List<Exception> _exceptions;
        
        /// <summary>
        /// Dodaje wyjątek.
        /// </summary>
        /// <param name="exception">Wyjątek do dodania.</param>
        public void AddException(Exception exception)
        {
            _exceptions.Add(exception);
        }

        /// <summary>
        /// Dodaje wiele wyjątków.
        /// </summary>
        /// <param name="exceptions">Lista wyjątków do dodania</param>
        public void AddExceptions(List<Exception> exceptions)
        {
            foreach (Exception exception in exceptions)
            {
                AddException(exception);
            }
        }

        /// <summary>
        /// Pobiera wyjątki.
        /// </summary>
        /// <returns>Lista wszystkich wyjątków</returns>
        public List<Exception> GetExceptions()
        {
            return _exceptions;
        }

        /// <summary>
        /// Informuje czy zakończono sukcesem.
        /// </summary>
        public bool Succeeded
        {
            get
            {
                return _exceptions.Count == 0; 
            }
        }

        /// <summary>
        /// Inicjalizuje wartości domyślne.
        /// </summary>
        public Result()
        {
            _exceptions = new List<Exception>();
        }

        /// <summary>
        /// Wypisuje wszystkie wyjątki (metoda do debugowania).
        /// </summary>
        public void PrintExceptions()
        {
            foreach (Exception exception in _exceptions)
            {
                PrintException(exception);
            }
        }

        /// <summary>
        /// Wypisuje jeden wyjątek (metoda do debugowania).
        /// </summary>
        /// <param name="exception">Wyjątek do wypisania</param>
        public static void PrintException(Exception exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Error Message: ");
            stringBuilder.Append(exception.Message);
            stringBuilder.Append("\nInner Exception: ");
            stringBuilder.Append(exception.InnerException);
            stringBuilder.Append("\nCallStack: ");
            stringBuilder.Append(exception.StackTrace);
            stringBuilder.Append('\n');
            Debug.WriteLine(stringBuilder.ToString());
        }
    }
}
