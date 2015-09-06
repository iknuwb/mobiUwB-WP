#region

using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using MobiUwB.Controls.RoundButtons;
using MobiUwB.Resources;
using MobiUwB.StartupConfig;

#endregion

namespace MobiUwB.Views.About
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();

            List<RoundButtonModel> authors = new List<RoundButtonModel>();

            //licencja 
            // \uE1DE hacker shield
            // \U0001F6C2 policeman

            //podziękowanie
            // \U0001F465

            foreach (String tutor in StartupConfiguration.Configuration.Tutors.TutorsList)
            {
                authors.Add(
                    new RoundButtonModel(
                        tutor  + ' ' + AppResources.AboutPageTutorEnding,
                        "\uE2AF",
                        tutor));
            }

            foreach (String author in StartupConfiguration.Configuration.Authors.AuthorsList)
            {
                authors.Add(
                    new RoundButtonModel(
                        author,
                        "\uE2AF", 
                        author));
            }

            AuthorsListBox.DataContext = authors;

            GreetingsButton.DataContext = "\U0001F465";

            LicenseButton.DataContext = "\uE1DE";
        }
    }
}