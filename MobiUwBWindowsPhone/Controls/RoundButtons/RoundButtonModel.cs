#region

using System;

#endregion

namespace MobiUwB.Controls.RoundButtons
{
    /// <summary>
    /// Model przycisku z zaukrąglonymi rogami
    /// </summary>
    public class RoundButtonModel
    {
        private String _text;
        /// <summary>
        /// Pobiera lub ustawia wartość wyświetlanego tekstu.
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private String _imageText;
        /// <summary>
        /// Pobiera lub ustawia wartość unicode wyświetlanego obrazka.
        /// </summary>
        public String ImageText
        {
            get { return _imageText; }
            set { _imageText = value; }
        }

        private Object _model;
        /// <summary>
        /// Pobiera lub ustawia wartość modelu, modelu przycisku.
        /// </summary>
        public Object Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public RoundButtonModel(String text, String imageText, Object model)
        {
            _text = text;
            _imageText = imageText;
            _model = model;
        }
    }
}
