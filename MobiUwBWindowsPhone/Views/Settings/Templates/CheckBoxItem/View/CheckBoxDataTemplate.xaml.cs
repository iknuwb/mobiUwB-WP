#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;
using MobiUwB.Views.Settings.Templates.CheckBoxItem.Model;

#endregion

namespace MobiUwB.Views.Settings.Templates.CheckBoxItem.View
{
    public partial class CheckBoxDataTemplate : UserControl
    {
        public CheckBoxDataTemplate()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckBoxTemplateModel d = (CheckBoxTemplateModel)DataContext;
            d.IsChecked = !d.IsChecked;
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
