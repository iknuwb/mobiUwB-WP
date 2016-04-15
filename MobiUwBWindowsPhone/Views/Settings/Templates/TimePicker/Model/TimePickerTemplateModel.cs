#region

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Controls;

#endregion

namespace MobiUwB.Views.Settings.Templates.TimePicker.Model
{
    /// <summary>
    /// Reprezentuje akcję walidacji danych wybranych z timepickera.
    /// </summary>
    /// <param name="dt">Nowo wybrana wartość</param>
    /// <returns>Informacja czy jest poprawna z założeniami.</returns>
    public delegate bool ValidateEvent(DateTime dt);

    /// <summary>
    /// Model templatea TimePickera.
    /// </summary>
    [DataContract]
    public class TimePickerTemplateModel : TemplateModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Występuje gdy zmieni się wartość czasu wybranego na TimePickerze.
        /// </summary>
        public event ValidateEvent Validate;

        /// <summary>
        /// Występuje gdy zmieni się wartość zmiennej.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _value;
        [DataMember]
        public DateTime Value
        {
            get { return _value; }
            set
            {
                bool isValid = true;
                if (Validate != null)
                {
                    isValid = Validate(value);
                }
                if (isValid)
                {
                    _value = value;
                }
                PropChanged("Value");
            }
        }

        /// <summary>
        /// Informuje, że wartość zmiennej uległa zmianie.
        /// </summary>
        /// <param name="propName">Nazwa zmiennej, która uległa zmianie.</param>
        private void PropChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
