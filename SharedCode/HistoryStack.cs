using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode
{
    /// <summary>
    /// Zarządza historią odwiedzanych stron.
    /// </summary>
    public class HistoryStack
    {
        /// <summary>
        /// Zawiera wsyztskie odwiedzane strony.
        /// </summary>
        private Stack<Uri> _historyStack;
        /// <summary>
        /// Poprzednio odwiedzona strona.
        /// </summary>
        private Uri _previous;
        /// <summary>
        /// Index poprzednio odwiedzonej strony
        /// </summary>
        private int _previousIndex;
        /// <summary>
        /// Index obecnej strony
        /// </summary>
        private int _currentIndex;
        
        /// <summary>
        /// Inicjalizuje domyślne wartości.
        /// </summary>
        public HistoryStack()
        {
            _historyStack = new Stack<Uri>();
            _currentIndex = -1;
            _previousIndex = -1;
        }

        /// <summary>
        /// Pobiera element ze szczytu stosu bez usuwania.
        /// </summary>
        /// <returns>Pobrany element</returns>
        public Uri Peek()
        {
            if (_historyStack.Count != 0)
            {
                return _historyStack.Peek();
            }
            return null;
        }

        /// <summary>
        /// Pobiera element ze szczytu i usuwa go.
        /// </summary>
        /// <returns>Pobrany element</returns>
        public Uri Pop()
        {
            _previous = Peek();

            if(_previous != null)
            { 
                _previousIndex = _currentIndex;
                _currentIndex--;

                return _historyStack.Pop();
            }
            return null;
        }

        /// <summary>
        /// Dodaje element na górę stosu.
        /// </summary>
        /// <param name="item">Element do dodania</param>
        public void Push(Uri item)
        {
            if (item == null)
            {
                return;
            }

            Uri current = Peek();
            if (current != null &&
                current.ToString().Equals(item.ToString()))
            {
                return;
            }

            _previousIndex = _currentIndex;
            _currentIndex = _historyStack.Count;
            if (_currentIndex > _previousIndex)
            {
                _previous = current;
                _historyStack.Push(item);
            }
        }

        /// <summary>
        /// Czyści historię.
        /// </summary>
        public void Clear()
        {
            _historyStack.Clear();
            _previousIndex = -1;
            _currentIndex = -1;
            _previous = null;
        }

        /// <summary>
        /// Przekształca stos na tablicę.
        /// </summary>
        /// <returns>Tablica odwiedzanych stron</returns>
        public Uri[] ToArray()
        {
            return _historyStack.ToArray();
        }
    }
}
