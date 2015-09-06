#region

using System;

#endregion

namespace MobiUwB.Utilities
{
    /// <summary>
    /// Zarządza konwersją jednostek.
    /// </summary>
    public class UnitConventer
    {
        /// <summary>
        /// Zmienia piksele na rozmiar czcionki.
        /// </summary>
        /// <param name="pixels">Ilośc pikseli</param>
        /// <returns>Rozmiar czcionki</returns>
        public static Double PixelsToFontSize(Double pixels)
        {
            return 3d / 4d * pixels;
        }

        /// <summary>
        /// Zmienia rozmiar czcionki na piksele
        /// </summary>
        /// <param name="points">Rozmiar czcionki</param>
        /// <returns>Piksele</returns>
        public static Double FontSizeToPixels(Double points)
        {
            return 4d / 3d * points;
        }
    }
}
