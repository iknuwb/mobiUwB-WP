#region

using System;
using System.Windows;
using MobiUwB.Views.Settings.Templates.CheckBoxItem.Model;
using MobiUwB.Views.Settings.Templates.ListPicker.Model;
using MobiUwB.Views.Settings.Templates.SwitchItem.Model;
using MobiUwB.Views.Settings.Templates.TimePicker.Model;

#endregion

namespace MobiUwB.Views.Settings.Templates.Selector
{
    /// <summary>
    /// Zarządza template'ami ekranu settings.
    /// </summary>
    public class SettingsTemplateSelector : TemplateSelector
    {
        private DataTemplate _listPickerTemplate;
        /// <summary>
        /// Pobiera lub nadaje template ListPickera
        /// </summary>
        public DataTemplate ListPickerTemplate
        {
            get { return _listPickerTemplate; }
            set { _listPickerTemplate = value; }
        }

        private DataTemplate _switchTemplate;
        /// <summary>
        /// Pobiera lub nadaje template Switcha
        /// </summary>
        public DataTemplate SwitchTemplate
        {
            get { return _switchTemplate; }
            set { _switchTemplate = value; }
        }

        private DataTemplate _checkBoxTemplate;
        /// <summary>
        /// Pobiera lub nadaje template CheckBoxa
        /// </summary>
        public DataTemplate CheckBoxTemplate
        {
            get { return _checkBoxTemplate; }
            set { _checkBoxTemplate = value; }
        }

        private DataTemplate _timePickerTemplate;
        /// <summary>
        /// Pobiera lub nadaje template TimePickera
        /// </summary>
        public DataTemplate TimePickerTemplate
        {
            get { return _timePickerTemplate; }
            set { _timePickerTemplate = value; }
        }

        /// <summary>
        /// Wybiera odpowiedni template na podstawie obiektu.
        /// </summary>
        /// <param name="item">Obiektm na podstawie którego wybierany jest template</param>
        /// <param name="container">Kontener</param>
        /// <returns>Wybrany template</returns>
        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            if (item is SwitchTemplateModel)
            {
                return _switchTemplate;
            }
            if (item is CheckBoxTemplateModel)
            {
                return _checkBoxTemplate;
            }
            if (item is TimePickerTemplateModel)
            {
                return _timePickerTemplate;
            }
            if (item is IListPickerTemplateModel)
            {
                return _listPickerTemplate;
            }
            throw new ArgumentException("Unsuported Template");
        }
    }
}
