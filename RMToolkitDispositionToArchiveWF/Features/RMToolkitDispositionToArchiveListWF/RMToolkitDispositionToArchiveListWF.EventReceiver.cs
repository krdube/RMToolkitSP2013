using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
namespace RMToolkitDispositionToArchiveWF.Features.RMToolkitDispositionToArchiveListWF
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("660a3833-ef6c-4dd4-a100-b4f0864a5167")]
    public class RMToolkitDispositionToArchiveListWFEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        //from professional sharepoint 2010 development (THomas Rizzo)
        {
            SPSite site = (SPSite)properties.Feature.Parent;
            
            //DisposeCheckOK
            SPWeb web = site.RootWeb;
            try
            {
                string Archived = web.Fields.Add("Archived", SPFieldType.Boolean, false);
                web.Update();

                SPField archivedField = web.Fields[Archived];
                archivedField.Title = "Archived";
                archivedField.Description = "Indicates that the record has been archived";
                archivedField.Group = "RMToolkit";
                archivedField.Update();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }


            
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            //if (properties.Feature.FeatureDefinitionScope != SPFeatureDefinitionScope.Site)
            //{
            //    throw new Exception("This feature must be a 'Site' feature");
            //}
            SPSite site = (SPSite)properties.Feature.Parent;
            
            //DisposeCheckOK
            SPWeb web = site.RootWeb;
                
            try
            {
                web.Fields["Archived"].Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


                

            


        }


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
