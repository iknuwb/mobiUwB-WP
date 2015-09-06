

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MobiUwB.Connection;
using MobiUwB.StartupConfig;
using MobiUwB.Utilities;
using SharedCode.Parsers;
using SharedCode;
using SharedCode.Utilities;

#endregion

namespace MobiUwB
{
    public partial class MainPage : PhoneApplicationPage
    {
        private WebBrowserState _webBrowserState;
        private HistoryStack _historyStack; 

        public MainPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            DisableMainScreen();

            List<InstitiuteModel> institiutesLst = 
                ParserFactory.GenerateInstituteModels(
                    StartupConfiguration.Properties.Websites.WebsiteList, 
                    @"Assets/logouwb.png");

            InstitutesListBox.DataContext = institiutesLst;

            InstitutesListBox.SelectedItem =
                ParserFactory.FindWrapperBy(
                    StartupConfiguration.Properties.Websites.DefaultWebsite,
                    institiutesLst); 
        }

        private void Initialize()
        {
            _historyStack = new HistoryStack();
            StartPivot.Tag = PivotItemType.Start;
            InstitiutesPivot.Tag = PivotItemType.Institiutes;
        }


        private void SetEnabledForApplicationBarImageButtons(Boolean isEnabled)
        {
            foreach (ApplicationBarIconButton button in ApplicationBar.Buttons)
            {
                button.IsEnabled = isEnabled;
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (MainWebBrowser.Source != null)
            {
                MainWebBrowser.Navigate(MainWebBrowser.Source);
            }
        }


        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            string pingPage = StartupConfiguration.Properties.Websites.DefaultWebsite.Ping;

            if (MainWebBrowser.Source == null)
            {
                if (_webBrowserState == WebBrowserState.Failed)
                {
                    e.Cancel = true;
                    BrowserGoBack();
                }
            }
            else if (MainWebBrowser.Source.OriginalString != pingPage)
            {
                e.Cancel = true;
                if (MainWebBrowser.CanGoBack)
                {
                    e.Cancel = true;
                    BrowserGoBack();
                }
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }

        private void BrowserGoBack()
        {
            #if DEBUG
            Uri[] array = _historyStack.ToArray();
            #endif
            _historyStack.Pop();
            Uri previousPage = _historyStack.Peek();
            MainWebBrowser.Navigate(previousPage);
        }

        private void Settings_MenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings/SettingsPage.xaml", UriKind.Relative));
        }


        private void Contact_MenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Contact/ContactPage.xaml", UriKind.Relative));
        }


        private void About_MenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About/AboutPage.xaml", UriKind.Relative));
        }


        private void Close_MenuItem_Click(object sender, EventArgs e)
        {
            Application.Current.Terminate(); 
        }


        private void Institutes_ListBox_SelectionChanged(
            object sender, 
            SelectionChangedEventArgs e)
        {
            if (InstitutesListBox.SelectedItem == null)
            {
                return;
            }
            InstitiuteModel selectedItem = 
                (InstitiuteModel)InstitutesListBox.SelectedItem;

            if (!selectedItem.Website.Equals(
                StartupConfiguration.Properties.Websites.DefaultWebsite))
            {
                StartupConfiguration.Properties.Websites.DefaultWebsite = 
                    selectedItem.Website;

                Globals.CurrentUnitId = selectedItem.Website.Id;
                UnitIdStorer unitIdStorer = new UnitIdStorer();
                unitIdStorer.RunWorkerAsync(Globals.CurrentUnitId);

                try
                {
                    App.XmlParser.Serialize(
                        StartupConfiguration.Properties,
                        StartupConfiguration.PropertiesFileName);
                }
                catch (Exception exception)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Error Message: ");
                    stringBuilder.Append(exception.Message);
                    stringBuilder.Append("\nInner Exception: ");
                    stringBuilder.Append(exception.InnerException);
                    stringBuilder.Append("\nCallStack: ");
                    stringBuilder.Append(exception.StackTrace);

                    Debug.WriteLine(stringBuilder.ToString());
                }
            }

            Pivot.SelectedItem = StartPivot;
            MainWebBrowser.Navigate(new Uri(selectedItem.Page));
        }

        private void EnableMainScreen()
        {
            MainWebBrowser.Opacity = 1;
            SetEnabledForApplicationBarImageButtons(true);
            ProgressIndicatorScreen.Visibility = Visibility.Collapsed;
        }

        private void DisableMainScreen()
        {
            MainWebBrowser.Opacity = 0;
            SetEnabledForApplicationBarImageButtons(false);
            ProgressIndicatorScreen.Visibility = Visibility.Visible;
        }

        private void Pivot_SelectionChanged(
            object sender, 
            SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            PivotItem selectedItem = pivot.SelectedItem as PivotItem;
            if (selectedItem != null)
            {
                PivotItemType? piwotType = selectedItem.Tag as PivotItemType?;
                switch (piwotType)
                {
                    case PivotItemType.Institiutes:
                    {
                        SetEnabledForApplicationBarImageButtons(false);
                        break;
                    }
                    case PivotItemType.Start:
                    {
                        SetEnabledForApplicationBarImageButtons(true);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }

        private void MainWebBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            DisableMainScreen();
            _webBrowserState = WebBrowserState.Navigating;

            if (e.Uri.AbsolutePath.EndsWith(".pdf") &&
                !e.Uri.AbsolutePath.StartsWith("http://docs.google.com/gview?embedded=true&url="))
            {
                MainWebBrowser.Navigate(new Uri("http://docs.google.com/gview?embedded=true&url=" + e.Uri));
                e.Cancel = true;
            }
            else
            {
                _historyStack.Push(e.Uri);
            }
        }

        private void MainWebBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
            _webBrowserState = WebBrowserState.Navigated;
        }

        private void Main_WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _webBrowserState = WebBrowserState.Navigated;
            EnableMainScreen();
        }

        private void MainWebBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            _webBrowserState = WebBrowserState.Failed;
            EnableMainScreen();
        }
    }
}