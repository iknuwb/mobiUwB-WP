#region

using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Phone.Controls;
using MobiUwB.Connection;
using MobiUwB.StartupConfig;
using MobiUwB.StartupConfig.Worker;

#endregion

namespace MobiUwB.Views.SplashScreen
{
    public partial class SplashScreenPage : PhoneApplicationPage
    {
        private StartupConfiguration _startupConfiguration;

        public SplashScreenPage()
        {
            InitializeComponent();
        }

        private void LayoutRoot_OnLoaded(
            object sender, 
            RoutedEventArgs e)
        {
            _startupConfiguration = new StartupConfiguration();
            _startupConfiguration.Finished += StartupConfigurationOnFinished;
            _startupConfiguration.startConfiguration();
        }

        private void StartupConfigurationOnFinished(
            object sender, 
            RunWorkerCompletedEventArgs eventArgs)
        {
            StartupConfigurationResult configurationResult = 
                (StartupConfigurationResult) eventArgs.Result;

            if (configurationResult.Succeeded)
            {
                NavigationService.Navigate(
                    new Uri("/MainPage.xaml",
                        UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            else
            {
                NavigationService.Navigate(
                    new Uri("/Views/SomethingWrong/SomethingWrongPage.xaml",
                        UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
        }
    }
}