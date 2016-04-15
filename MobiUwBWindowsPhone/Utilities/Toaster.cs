#region

using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;

#endregion

namespace MobiUwB.Utilities
{
    /// <summary>
    /// Zarządza tworzeniem toast'ów.
    /// </summary>
    public static class Toaster
    {
        /// <summary>
        /// Przechowuję kierunek ułożenia tekstu wewnątrz toastu.
        /// </summary>
        private const Orientation TextOrientation = Orientation.Horizontal;

        /// <summary>
        /// Pokazuje na ekranie toast.
        /// </summary>
        /// <param name="title">Tytuł toast'u</param>
        /// <param name="message">Wiadomość toast'u</param>
        /// <param name="bitmapImage">Obrazek toast'u</param>
        public static void Make(
            String title,
            String message = "",
            BitmapImage bitmapImage = null)
        {
            ToastPrompt toastPrompt = new ToastPrompt();
            toastPrompt.Title = title;
            toastPrompt.Message = message;
            toastPrompt.TextOrientation = TextOrientation;

            if (bitmapImage != null)
            {
                toastPrompt.ImageSource = bitmapImage;
            }
            toastPrompt.Show();
        }
    }
}
