#region

using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;

#endregion

namespace MobiUwB.Utilities
{
    public static class ToastManager
    {
        private const Orientation TextOrientation = Orientation.Horizontal;

        public static void ShowToast(
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
