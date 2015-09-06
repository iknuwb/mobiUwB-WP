#region

using System.Windows;
using System.Windows.Controls;

#endregion

namespace MobiUwB.Views.Settings.Templates.Selector
{
    /// <summary>
    /// Zarządza wyborem odpowiedniego template'a.
    /// </summary>
    public abstract class TemplateSelector : ContentControl
    {
        /// <summary>
        /// Wybiera odpowiedni template na podstawie dostarczonego obiektu.
        /// </summary>
        /// <param name="item">Obiekt, do którego dopasowujemy template.</param>
        /// <param name="container">Contener zawierający</param>
        /// <returns>Wybrany template</returns>
        public abstract DataTemplate SelectTemplate(object item, DependencyObject container);

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}
