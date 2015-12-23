using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;

namespace RMToolkitTimer.Features.RMToolkitTimerFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7c463a69-adc8-444b-b5c2-92f1e3421fc9")]
    public class RMToolkitTimerFeatureEventReceiver : SPFeatureReceiver
    {
        const string JobName = "RMToolkitTimerJob";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;

            DeleteJob(webApp.JobDefinitions); // Delete Job if already Exists

            CreateJob(webApp); // Create new Job
        }

        //private static void DeleteJob(SPSite site)
        //{
        //    foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
        //        if (job.Name == JobName)
        //            job.Delete();
        //}

        private void DeleteJob(SPJobDefinitionCollection jobs)
        {
            foreach (SPJobDefinition job in jobs)
            {
                if (job.Name.Equals(RMToolkitTimerJob.JobName,
                StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }
                if (job.Name.Equals("RMTookitTimerJob", StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }
                if (job.Name.Equals("RMToolkit TimerJob", StringComparison.OrdinalIgnoreCase))
                {
                    job.Delete();
                }

            }
        }


        private static void CreateJob(SPWebApplication webApp)
        {
            RMToolkitTimerJob job = new RMToolkitTimerJob(webApp);

            // Create the schedule so that the job runs daily, sometime between 
            // midnight and 4 A.M.
            SPDailySchedule schedule = new SPDailySchedule();
            schedule.BeginHour = 0;
            schedule.BeginMinute = 0;
            schedule.BeginSecond = 0;
            schedule.EndHour = 3;
            schedule.EndMinute = 59;
            schedule.EndSecond = 59;
            job.Schedule = schedule;
            job.Update();

        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
            DeleteJob(webApp.JobDefinitions); // Delete the Job
        }


        // Uncomment the method below to handle the event raised after a feature has been activated.

        //public override void FeatureActivated(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
