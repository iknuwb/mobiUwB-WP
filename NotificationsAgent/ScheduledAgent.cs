#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using NotificationsAgent.DataInitialize;
using SharedCode.Tasks;
using SharedCode.VersionControl;
using SharedCode.VersionControl.Models;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models;
using SharedCode.Parsers.Json.Model;
using NotificationsAgent.DataAccess;
using SharedCode.DataManagment;

using Section = SharedCode.Parsers.Models.ConfigurationXML.Section;
using Windows.Storage;
using System.Threading.Tasks;
using SharedCode.Parsers.Json;
using SharedCode.Parsers;
using NotificationsAgent.DataInitialize.Tasks.PropertiesXml;
using NotificationsAgent.DataInitialize.Tasks.VersionController;
using NotificationsAgent.DataInitialize.Tasks.ConfigurationXml;
using NotificationsAgent.DataInitialize.Tasks.CategoriesFinder;
using NotificationsAgent.DataInitialize.Tasks.UnitIdFinder;

#endregion

namespace NotificationsAgent
{
    /// <summary>
    /// Zarządza usługą.
    /// </summary>
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <summary>
        /// Dostęp do zarządzania danymi.
        /// </summary>
        public static readonly DataManager DataManager;
        /// <summary>
        /// Dostęp do zapisu plików.
        /// </summary>
        public static readonly WriteData WriteData;
        /// <summary>
        /// Dostęp do odczytu plików.
        /// </summary>
        public static readonly ReadData ReadData;
        /// <summary>
        /// Globalny parser XML.
        /// </summary>
        public static readonly XmlParser XmlParser;
        /// <summary>
        /// Dostęp do zarządzania plikami.
        /// </summary>
        public static readonly IoManager IoManager;
        /// <summary>
        /// Nazwa pliku properties.
        /// </summary>
        public static readonly String PropertiesFileName = "properties.xml";
        /// <summary>
        /// Nazwa pliku configuracyjnego.
        /// </summary>
        public static readonly String ConfigurationFileName = "configuration.xml";
        /// <summary>
        /// Obiekt klasy zarządzającej procesem zbierania danych.
        /// </summary>
        private DataInitializeTaskOutput _dataInitializeTaskOutput;
        /// <summary>
        /// Czas osttatniego wykonania sprawdzenia.
        /// </summary>
        private long _lastCheckTime;

        /// <summary>
        /// Inicjacja wartości domyślnych oraz, nadawanie eventów.
        /// </summary>
        static ScheduledAgent()
        {
            WriteData = new WriteData();
            ReadData = new ReadData();
            DataManager = new DataManager(ReadData, WriteData);
            IoManager = new IoManager();
            XmlParser = new XmlParser(IoManager);

            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// <summary>
        /// Obsługuje nieobsłużone wyjątki.
        /// </summary>
        /// <param name="sender">Obiekt wywołujący</param>
        /// <param name="e">Obiekt wyjątku</param>
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Wowułuje usługę.
        /// </summary>
        /// <param name="task">Wywołana usługa</param>
        protected override void OnInvoke(ScheduledTask task)
        {
            bool succeeded = InitializeData();

            ProcessNotifications(succeeded);

            NotifyComplete();
        }

        /// <summary>
        /// Tworzy powiadomienie o padym tytule oraz treści.
        /// </summary>
        /// <param name="title">Tytuł powiadomienia</param>
        /// <param name="content">Wiadomość powiadomienia</param>
        /// <returns>Utoworzne powiadomienie</returns>
        private ShellToast CreateShellToast(String title, String content)
        {
            ShellToast toast = new ShellToast();
            toast.Title = title;
            toast.Content = content;
            return toast;
        }

        /// <summary>
        /// Uruchamia proces przetwarzania powiadomień.
        /// </summary>
        /// <param name="succeeded">Zmienna wejściowa czy sukces</param>
        private void ProcessNotifications(bool succeeded)
        {
            if(succeeded)
            {
                NotificationsLoop();
            }
        }

        /// <summary>
        /// Wykonuje zapętlone sprawdzanie powiadomień,
        /// </summary>
        private void NotificationsLoop()
        {
            long interval = _dataInitializeTaskOutput.interval;
            DateTime from = _dataInitializeTaskOutput.timeRangeFrom;
            DateTime to = _dataInitializeTaskOutput.timeRangeTo;
            while (_dataInitializeTaskOutput.isNotificationActive)
            {
                NotificationsLoopTick(interval, from, to);
            }
        }

        /// <summary>
        /// Sprawdza odstęp czasowy został przekroczony.
        /// </summary>
        /// <param name="interval">Odstęp czasowy</param>
        /// <param name="from">Czas od</param>
        /// <param name="to">Czas do</param>
        private void NotificationsLoopTick(long interval, DateTime from, DateTime to)
        {
            if(DateTime.Now.Ticks > _lastCheckTime + interval)
            {
                if(_dataInitializeTaskOutput.isTimeRangeActive)
                {
                    TimeRangeNotificationsExecute(from, to);
                }
                else
                {
                    NotificationsExecute();
                }
            }
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Wywołuje sprawdzenie powiadomień.
        /// </summary>
        private void NotificationsExecute()
        {
            _lastCheckTime = DateTime.Now.Ticks;
            RunNotificationsPublisher();
        }

        /// <summary>
        /// Sprawdza zakres czasowy.
        /// </summary>
        /// <param name="from">Czas od</param>
        /// <param name="to">Czas do</param>
        private void TimeRangeNotificationsExecute(DateTime from, DateTime to)
        {
            DateTime currentDate = new DateTime(DateTime.Now.Ticks);
            int currentHours = currentDate.Hour;
            int currentMinutes = currentDate.Minute;

            int fromHours = from.Hour;
            int fromMinutes = from.Minute;

            int toHours = to.Hour;
            int toMinutes = to.Minute;

            if(fromHours != toHours)
            {
                NotificationsTimeRangeCheck(currentHours,
                                            fromHours,
                                            toHours);
            }
            else
            {
                NotificationsTimeRangeCheck(currentMinutes,
                                            fromMinutes,
                                            toMinutes);
            }
        }

        /// <summary>
        /// Sprawdza zakres godzinowy.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="from">Czas od</param>
        /// <param name="to">Czas do</param>
        private void NotificationsTimeRangeCheck(int current, int from, int to)
        {
            if (from <= current &&
                    current <= to)
            {
                NotificationsExecute();
            }
        }


        /// <summary>
        /// Uruchamia proces sprawdzania powiadomień.
        /// </summary>
        private void RunNotificationsPublisher()
        {
            Unit unit = _dataInitializeTaskOutput.configXmlResult.GetUnitById(
                _dataInitializeTaskOutput.CurrentUnitId);
            int i = 0;
            foreach (KeyValuePair<String, Boolean> category in _dataInitializeTaskOutput.categories)
            {
                if (category.Value)
                {
                    Section section = unit.Sections.GetSectionById(category.Key);
                    List<Feed> newestFeeds = GetNewElements(section, unit);
                    PublishNotifications(newestFeeds, section, i);

                    if (newestFeeds.Count > 0)
                    {
                        RestolableDateTime restolableDateTime = new RestolableDateTime(DateTime.Now);

                        DataManager.StoreData(
                            restolableDateTime,
                            ConcatLastKnownDateId(unit.Id, section.SectionId));
                    }
                }
                i++;
            }
        }

        /// <summary>
        /// Pobiera najnowsze informacje dla danej sekcji.
        /// </summary>
        /// <param name="section">Kategoria dla kórej pobieramy</param>
        /// <param name="unit">Wydział z którego pobieramy</param>
        /// <returns>Lista najnowszych informacji</returns>
        private List<Feed> GetNewElements(Section section, Unit unit)
        {
            VersionController versionController = 
                new VersionController(
                    IoManager, 
                    ReadData, 
                    WriteData);
            String feedsFileUri = unit.ApiUrlString + section.SectionId;

            String folder = ApplicationData.Current.LocalFolder.Path;
            String fullSavePath = Path.Combine(
                folder,
                section.SectionId);

            VersioningRequest versioningRequest =
                    new VersioningRequest(
                        feedsFileUri,
                        fullSavePath);

            Task<VersioningResult> versioningResultTask = 
                versionController.GetNewestFile(versioningRequest);

            VersioningResult versioningResult = versioningResultTask.Result;

            List<Feed> newestFeeds = new List<Feed>();
            if (versioningResult.Succeeded)
            {
                String jsonContent = versioningResult.GetFileContent();

                JsonParser jsonParser = new JsonParser();

                List<Feed> notificationElements = 
                    jsonParser.ParseFeedsJson(jsonContent);

                RestolableDateTime lastKnownDate =
                    DataManager.RestoreData<RestolableDateTime>(ConcatLastKnownDateId(unit.Id,section.SectionId));

                foreach (Feed feed in notificationElements)
                {
                    if (feed.DateTime > lastKnownDate.DateTime)
                    {
                        newestFeeds.Add(feed);
                    }
                }
            }
            return newestFeeds;
        }

        /// <summary>
        /// Tworzy unikalne ID.
        /// </summary>
        /// <param name="unitId">ID wydziału</param>
        /// <param name="sectionId">ID kategorii</param>
        /// <returns></returns>
        private string ConcatLastKnownDateId(String unitId, String sectionId)
        {
            return "Unit_" + unitId + "__" + "Section_" + sectionId;
        }

        /// <summary>
        /// Przygotowuje wszystkie dane wejściowe.
        /// </summary>
        /// <returns>Informacja czy zakończona sukcesem</returns>
        private bool InitializeData()
        {

            TasksQueue<DataInitializeTaskOutput> tasksQueue =
                    new TasksQueue<DataInitializeTaskOutput>();

            _dataInitializeTaskOutput = new DataInitializeTaskOutput();

            UnitIdFinderTask unitIdFinderTask = new UnitIdFinderTask();
            tasksQueue.add(unitIdFinderTask, null);

            PropertiesXmlTask propertiesXmlTask = new PropertiesXmlTask();
            PropertiesXmlTaskInput propertiesXmlTaskInput = new PropertiesXmlTaskInput(
                "properties.xml");
            tasksQueue.add(propertiesXmlTask, propertiesXmlTaskInput);


            VersionControllerTask versionControllerTask = new VersionControllerTask();
            VersionControllerTaskInput versionControllerTaskInput =
                new VersionControllerTaskInput(
                    "config.xml");
            tasksQueue.add(versionControllerTask, versionControllerTaskInput);


            ConfigurationXmlTask configurationXmlTask = new ConfigurationXmlTask();
            tasksQueue.add(configurationXmlTask, null);


            CategoriesFinderTask settingsPreferenceManagerTask =
                new CategoriesFinderTask();
            tasksQueue.add(settingsPreferenceManagerTask, null);


            tasksQueue.performAll(_dataInitializeTaskOutput);
            _dataInitializeTaskOutput.PrintExceptions();
            return _dataInitializeTaskOutput.Succeeded;
        }

        /// <summary>
        /// Publikuje powiadomienia.
        /// </summary>
        /// <param name="newestFeeds">Lista nowych informacji</param>
        /// <param name="section">Wydział dla którego powiadamiamy</param>
        /// <param name="notificationId">ID powiadomeinia</param>
        private void PublishNotifications(List<Feed> newestFeeds,
                                          Section section,
                                          int notificationId)
        {
            int feedsAmount = newestFeeds.Count;
            if (feedsAmount > 0)
            {
                ShellToast toast = CreateShellToast(section.SectionTitle,  
                    Resources.Resources.NotificationContentText + feedsAmount);
                toast.Show();
            }
        }

    }
}