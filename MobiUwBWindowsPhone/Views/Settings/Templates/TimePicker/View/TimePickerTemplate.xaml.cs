#region

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

#endregion

namespace MobiUwB.Views.Settings.Templates.TimePicker.View
{
    public partial class TimePickerTemplate : UserControl
    {
        public TimePickerTemplate()
        {
            InitializeComponent();
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MyTimePicker.RaiseTap();
        }

    }



    public class TimePickerClickier : Microsoft.Phone.Controls.TimePicker
    {
        public void RaiseTap()
        {
            Button btn = (GetTemplateChild("DateTimeButton") as Button);
            ButtonAutomationPeer peer = new ButtonAutomationPeer(btn);
            IInvokeProvider provider = (peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider);

            provider.Invoke();
        }
    }
}
