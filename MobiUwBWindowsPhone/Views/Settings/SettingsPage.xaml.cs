#region

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MobiUwB.DataAccess;
using MobiUwB.StartupConfig;
using MobiUwB.Views.Settings.Templates;
using SharedCode.DataManagment;
using SharedCode.Utilities;
using System.Threading;
using Windows.Phone.Speech.VoiceCommands;
using MobiUwB.Resources;
using MobiUwB.Utilities;
using MobiUwB.Views.Settings.Templates.CheckBoxItem.Model;
using MobiUwB.Views.Settings.Templates.ListPicker.Model;
using MobiUwB.Views.Settings.Templates.SwitchItem.Model;
using MobiUwB.Views.Settings.Templates.TimePicker.Model;
using SharedCode;

#endregion

namespace MobiUwB.Views.Settings
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        static Random _radnom = new Random();
        private readonly DataManager _dataManager;

        private TimePickerTemplateModel _timePickerToTemplateModel;

        public TimePickerTemplateModel TimePickerToTemplateModel
        {
            get { return _timePickerToTemplateModel; }
            private set { _timePickerToTemplateModel = value; }
        }

        private TimePickerTemplateModel _timePickerFromTemplateModel;

        public TimePickerTemplateModel TimePickerFromTemplateModel
        {
            get { return _timePickerFromTemplateModel; }
            private set { _timePickerFromTemplateModel = value; }
        }

        public SettingsPage()
        {
            InitializeComponent();
            ReadData readData = new ReadData();
            WriteData writeData = new WriteData();
            _dataManager = new DataManager(readData, writeData);
        }

        private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            TemplateModel baseTemplateModel = _dataManager.RestoreData<TemplateModel>(
                StartupConfiguration.Properties.Websites.DefaultWebsite.Id);
            Expander.DataContext = baseTemplateModel;
            DistinguishTemplateModels(baseTemplateModel);
            TimePickerFromTemplateModel.Validate += TimePickerFromTemplateModel_Validate;
            TimePickerToTemplateModel.Validate += TimePickerToTemplateModel_Validate;
        }

        private void DistinguishTemplateModels(TemplateModel baseTemplateModel)
        {
            switch (baseTemplateModel.ID)
            {
                case Defaults.FromId:
                {
                    TimePickerFromTemplateModel = (TimePickerTemplateModel)baseTemplateModel;
                    break;
                }
                case Defaults.ToId:
                {
                    TimePickerToTemplateModel = (TimePickerTemplateModel)baseTemplateModel;
                    break;
                }
            }
            foreach (TemplateModel templateModel in baseTemplateModel.Children)
            {
                DistinguishTemplateModels(templateModel);
            }
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StoreModels();
            StoreModelsValues();
        }

        private void StoreModels()
        {
            _dataManager.StoreData(
                (TemplateModel)Expander.DataContext,
                StartupConfiguration.Properties.Websites.DefaultWebsite.Id);
        }

        private void StoreModelsValues()
        {
            using (Mutex mutex = new Mutex(true, Globals.CurrentUnitId))
            {
                mutex.WaitOne();
                try
                {
                    SerializeModelsValues();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private void SerializeModelsValues()
        {
            SettingsCategoriesValuesWrapper settingsCategoriesValuesWrapper;

            PrepareModelsValues(out settingsCategoriesValuesWrapper);

            IsolatedStorageFile isolatedStorageFile = 
                IsolatedStorageFile.GetUserStoreForApplication();

            using (IsolatedStorageFileStream isolatedStorageFileStream = 
                new IsolatedStorageFileStream(
                    Globals.CurrentUnitId, 
                    FileMode.OpenOrCreate, 
                    isolatedStorageFile))
            {
                DataContractJsonSerializer dataContractJsonSerializer = 
                    new DataContractJsonSerializer(
                        typeof(SettingsCategoriesValuesWrapper));
                
                dataContractJsonSerializer.WriteObject(
                    isolatedStorageFileStream, 
                    settingsCategoriesValuesWrapper);
            }
        }

        private void PrepareModelsValues(
            out SettingsCategoriesValuesWrapper settingsCategoriesValuesWrapper)
        {
            settingsCategoriesValuesWrapper = new SettingsCategoriesValuesWrapper();
            RecursiveFillModelsValues(
                settingsCategoriesValuesWrapper,
                (TemplateModel)Expander.DataContext);
        }

        private void RecursiveFillModelsValues(
            SettingsCategoriesValuesWrapper settingsCategoriesValuesWrapper,
            TemplateModel rootTemplateModel)
        {
            foreach (TemplateModel model in rootTemplateModel.Children)
            {
                String key = model.Text;
                object value = null;

                CheckBoxTemplateModel checkBoxTemplateModel = 
                    model as CheckBoxTemplateModel;

                SwitchTemplateModel switchTemplateModel = 
                    model as SwitchTemplateModel;

                ListPickerTemplateModel<Int64> int64ListPickerTemplateModel = 
                    model as ListPickerTemplateModel<Int64>;

                TimePickerTemplateModel timePickerTemplateModel = 
                    model as TimePickerTemplateModel;

                if(checkBoxTemplateModel != null)
                {
                    value = checkBoxTemplateModel.IsChecked;
                }
                else if(switchTemplateModel != null)
                {
                    value = switchTemplateModel.IsChecked;
                }
                else if(int64ListPickerTemplateModel != null)
                {
                    value = int64ListPickerTemplateModel.Value;
                }
                else if (timePickerTemplateModel != null)
                {
                    value = timePickerTemplateModel.Value;
                }

                settingsCategoriesValuesWrapper.AddValue(key, value);

                RecursiveFillModelsValues(
                    settingsCategoriesValuesWrapper,
                    model);
            }
        }
        private bool TimePickerFromTemplateModel_Validate(DateTime dt)
        {
            bool isValid = dt < TimePickerToTemplateModel.Value;
            if (!isValid)
            {
                Toaster.Make(AppResources.SettingsPageWrongTimeRange);
            }
            return isValid;
        }

        bool TimePickerToTemplateModel_Validate(DateTime dt)
        {
            bool isValid = TimePickerFromTemplateModel.Value < dt;
            if (!isValid)
            {
                Toaster.Make(AppResources.SettingsPageWrongTimeRange);
            }
            return isValid;
        }
    }
}