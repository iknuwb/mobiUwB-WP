#region

using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MobiUwB.Connection;
using MobiUwB.Controls.RoundButtons;
using MobiUwB.Resources;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion

namespace MobiUwB.Views.SomethingWrong
{
    public partial class SomethingWrongPage : PhoneApplicationPage
    {
        private InternetChecker _internetChecker;
        public SomethingWrongPage()
        {
            InitializeComponent();
            RoundButtonModel roundButtonModel = 
                new RoundButtonModel(
                    AppResources.SomethingWrongPageRefreshRoundButtonText,
                    "\U0000E117",
                    null);

            RefreshRoundButton.DataContext = roundButtonModel;


            _internetChecker = InternetChecker.GetInstance();
        }

        void InternetChecker_ConnectionReceived(InternetConnectionType internetConnectionType)
        {
            NavigateToSplashScreen();
        }

        private void RefreshRoundButton_Tap(object sender, GestureEventArgs e)
        {
            NavigateToSplashScreen();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _internetChecker.ConnectionReceived += InternetChecker_ConnectionReceived;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _internetChecker.ConnectionReceived -= InternetChecker_ConnectionReceived;
            base.OnNavigatedFrom(e);
        }

        private void NavigateToSplashScreen()
        {
            NavigationService.Navigate(
                new Uri("/Views/SplashScreen/SplashScreenPage.xaml",
                    UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}