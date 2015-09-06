#region

using System.Windows;
using System.Windows.Controls;
using MobiUwB.Views.Settings.Templates.SwitchItem.Model;

#endregion

namespace MobiUwB.Views.Settings.Templates.SwitchItem.View
{
    public partial class SwitchDataTemplate : UserControl
    {
        public SwitchDataTemplate()
        {
            InitializeComponent();
            Expander.Loaded += expander_Loaded;
            
        }

        void expander_Loaded(object sender, RoutedEventArgs e)
        {
            SwitchTemplateModel switchTemplateModel = (SwitchTemplateModel)Expander.Expander;
            Expander.IsExpanded = switchTemplateModel.IsChecked;
        }

        private void expander_Expanded(object sender, RoutedEventArgs e)
        {
            SwitchTemplateModel switchTemplateModel = (SwitchTemplateModel)Expander.Expander;
            switchTemplateModel.IsChecked = true;
        }

        private void expander_Collapsed(object sender, RoutedEventArgs e)
        {
            SwitchTemplateModel switchTemplateModel = (SwitchTemplateModel)Expander.Expander;
            switchTemplateModel.IsChecked = false;
        }
    }
}
