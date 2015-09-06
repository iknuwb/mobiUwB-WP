#region

using System.Device.Location;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using MobiUwB.Controls.RoundButtons;
using MobiUwB.Resources;
using MobiUwB.StartupConfig;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;
using SharedCode.Parsers.Models.ConfigurationXML;

#endregion

namespace MobiUwB.Views.Contact
{
    public partial class ContactPage : PhoneApplicationPage
    {
        private readonly Unit _unit;
        public ContactPage()
        {
            InitializeComponent();
            _unit = StartupConfiguration.Configuration.GetUnitById(
                StartupConfiguration.Properties.Websites.DefaultWebsite.Id);
            Initialize();
        }

        private void Initialize()
        {
            InitDescriptionPanel();

            InitEmailRoundButton();

            InitPhone1RoundButton();

            InitPhone2RoundButton();

            InitFaxRoundButton();

            InitLocalizationRoundButton();
        }

        private void InitDescriptionPanel()
        {
            TitleTextBlock.Text = _unit.Name;

            PostalCodeAndCityTextBlock.Text =
                _unit.Address.PostalCode +
                " " +
                _unit.Address.City;

            StreetTextBlock.Text =
                _unit.Address.Street +
                " " +
                _unit.Address.Number;
        }

        private void InitLocalizationRoundButton()
        {
            RoundButtonModel localization = new RoundButtonModel(
                AppResources.ContactPageLocalization, "\U0001F30E", _unit);
            LocalizationRoundButton.DataContext = localization;
        }

        private void InitFaxRoundButton()
        {
            RoundButtonModel fax = new RoundButtonModel(
                _unit.Fax, "\U0001F4E0", _unit);
            FaxRoundButton.DataContext = fax;
        }

        private void InitPhone2RoundButton()
        {
            if (_unit.Phone2 != null)
            {
                RoundButtonModel phone2 = new RoundButtonModel(
                    _unit.Phone2, "\uE13A", _unit);
                Phone2RoundButton.DataContext = phone2;
            }
            else
            {
                Phone2RoundButton.Visibility = Visibility.Collapsed;
            }
        }

        private void InitPhone1RoundButton()
        {
            RoundButtonModel phone1 = new RoundButtonModel(
                _unit.Phone1, "\uE13A", _unit);
            Phone1RoundButton.DataContext = phone1;
        }

        private void InitEmailRoundButton()
        {
            RoundButtonModel email = new RoundButtonModel(
                _unit.Email, "\U0001F4E7", _unit);
            EMailRoundButton.DataContext = email;
        }

        private void EMailRoundButton_OnTap(object sender, GestureEventArgs e)
        {
            RoundButton roundButton = (RoundButton)sender;
            RoundButtonModel model = (RoundButtonModel)roundButton.DataContext;
            Unit unit = (Unit)model.Model;

            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = unit.Email;
            emailComposeTask.Show();
        }

        private void Phone1RoundButton_OnTap(object sender, GestureEventArgs e)
        {
            RoundButton roundButton = (RoundButton)sender;
            RoundButtonModel model = (RoundButtonModel)roundButton.DataContext;
            Unit unit = (Unit)model.Model;

            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = unit.Phone1;
            phoneCallTask.DisplayName = unit.Name;
            phoneCallTask.Show();
        }

        private void Phone2RoundButton_OnTap(object sender, GestureEventArgs e)
        {
            RoundButton roundButton = (RoundButton)sender;
            RoundButtonModel model = (RoundButtonModel)roundButton.DataContext;
            Unit unit = (Unit)model.Model;

            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = unit.Phone2;
            phoneCallTask.DisplayName = unit.Name;
            phoneCallTask.Show();
        }

        private void FaxRoundButton_OnTap(object sender, GestureEventArgs e)
        {
            RoundButton roundButton = (RoundButton)sender;
            RoundButtonModel model = (RoundButtonModel)roundButton.DataContext;
            Unit unit = (Unit)model.Model;

            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = unit.Fax;
            phoneCallTask.DisplayName = unit.Name;
            phoneCallTask.Show();
        }

        private void LocalizationRoundButton_OnTap(object sender, GestureEventArgs e)
        {
            RoundButton roundButton = (RoundButton)sender;
            RoundButtonModel model = (RoundButtonModel)roundButton.DataContext;
            Unit unit = (Unit)model.Model;

            MapsTask mapsTask = new MapsTask();
            mapsTask.Center = new GeoCoordinate(
                    unit.Map.Coordinates.Longtitude,
                    unit.Map.Coordinates.Lattitude);
            mapsTask.ZoomLevel = 15;
            mapsTask.Show();
        }
    }
}