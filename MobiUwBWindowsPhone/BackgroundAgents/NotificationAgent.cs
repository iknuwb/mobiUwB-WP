
#define DEBUG_AGENT

#region

using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using MobiUwB.Resources;

#endregion

namespace MobiUwB.BackgroundAgents
{
    /// <summary>
    /// Zarządza usługą
    /// </summary>
    public class NotificationAgent
    {
        /// <summary>
        /// Przechowuje nazwę usługi
        /// </summary>
        const string Name = "PeriodicAgent";

        /// <summary>
        /// Informuje czy jakieś usługi są włączone
        /// </summary>
        public bool AgentsAreEnabled = true;

        /// <summary>
        /// Startuje działanie usługi
        /// </summary>
        public void Start()
        {
            PeriodicTask periodicTask = CreatePeriodicTask();

            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(periodicTask);
                // PeriodicStackPanel.DataContext = periodicTask;

                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG_AGENT)
                ScheduledActionService.LaunchForTest(Name, TimeSpan.FromSeconds(20));
#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show(AppResources.NotificationsBackgroundAgentNotAvailable);
                    AgentsAreEnabled = false;
                }
                Debug.WriteLine(exception);
            }
            catch (SchedulerServiceException exception)
            {
                Debug.WriteLine(exception);
            }
        }

        /// <summary>
        /// Tworzy obiekt usługi
        /// </summary>
        /// <returns>Nowy obiekt usługi</returns>
        private PeriodicTask CreatePeriodicTask()
        {
            // Obtain a reference to the period task, if one exists
            PeriodicTask periodicTask =
                ScheduledActionService.Find(Name) as PeriodicTask;

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null)
            {
                ScheduledActionService.Remove(Name);
            }

            periodicTask = new PeriodicTask(Name);

            // The description is required for periodic agents. This is the string that the user
            // will see in the background services Settings page on the device.
            periodicTask.Description = AppResources.NotificationsBackgroundAgentDescription;
            return periodicTask;
        }
    }
}
