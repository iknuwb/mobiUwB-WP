#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

namespace MobiUwB.Views.Settings.Templates.ListPicker.Model
{
    /// <summary>
    /// Model templatea list pickera.
    /// </summary>
    /// <typeparam name="TValue">Typ wartości możliwej do wyboru z listy</typeparam>
    [DataContract]
    public class ListPickerTemplateModel<TValue> : TemplateModel, INotifyPropertyChanged, IListPickerTemplateModel
    {
        private int _value;
        /// <summary>
        /// Pobiera lub nadaje wartość nadaną z listy.
        /// </summary>
        [DataMember]
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                PropChanged("Value");
            }
        }

        private List<ListPickerItem> _items;
        /// <summary>
        /// Pobiera dostęp do listy lub nadaje nową.
        /// </summary>
        [DataMember]
        public List<ListPickerItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// Nadaje wartości domyślne.
        /// </summary>
        public ListPickerTemplateModel()
        {
            _items = new List<ListPickerItem>();
            Value = 0;
        }

        /// <summary>
        /// Występuje gdy zmieni się wartość wybrana na liście/
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Występuje gdy zmieni się wartość zmiennej o podanej nazwie.
        /// </summary>
        /// <param name="propName">Nazwa property, które zmieniło wartość</param>
        private void PropChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Model pojedyńczego elementu wyświetlanego na liście.
        /// </summary>
        [DataContract]
        public class ListPickerItem
        {
            private String _title;
            /// <summary>
            /// Pobiera lub nadaje wyświetlany tekst.
            /// </summary>
            [DataMember]
            public String Title
            {
                get { return _title; }
                set { _title = value; }
            }

            private TValue _value;
            /// <summary>
            /// Wartość reprezentowana przez ten element listy.
            /// </summary>
            [DataMember]
            public TValue Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                }
            }

            /// <summary>
            /// Porównuje 2 obiekty.
            /// </summary>
            /// <param name="other">Obiekt, do którego chcemy porównać</param>
            /// <returns>Wynik porównania</returns>
            protected bool Equals(ListPickerItem other)
            {
                return Value.Equals(other.Value);
            }

            /// <summary>
            /// Porównuje 2 obiekty.
            /// </summary>
            /// <param name="obj">Obiekt, do którego chcemy porównać</param>
            /// <returns>Wynik porównania</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }
                if (obj.GetType() != GetType())
                {
                    return false;
                }
                return Equals((ListPickerItem) obj);
            }

            /// <summary>
            /// Pobiera HashCode obiektu.
            /// </summary>
            /// <returns>HashCode obiektu</returns>
            public override int GetHashCode()
            {
                return 7;
            }

            public ListPickerItem(String title, TValue value)
            {
                _title = title;
                _value = value;
            }

            /// <summary>
            /// Prezentuje obiekt w sposób czytelny dla człowieka.
            /// </summary>
            /// <returns>Opis czytelny dla człowieka.</returns>
            public override string ToString()
            {
                return Title;
            }
        }
    }
}
