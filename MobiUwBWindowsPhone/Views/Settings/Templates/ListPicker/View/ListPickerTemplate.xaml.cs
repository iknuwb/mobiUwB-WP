#region

using System.Windows.Controls;

#endregion

namespace MobiUwB.Views.Settings.Templates.ListPicker.View
{
    public partial class ListPickerTemplate : UserControl
    {
        public ListPickerTemplate()
        {
            InitializeComponent();
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListPicker.Open();
        }
    }
}
