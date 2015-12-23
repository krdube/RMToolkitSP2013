using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace RMTookitExpiryWF.Features.RMToolkitExpiryWF
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("b932f5a1-a337-43b5-aa96-d06d537c762e")]
    public class RMToolkitExpiryWFEventReceiver : SPFeatureReceiver
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
                string ExpiryDate = web.Fields.Add("ExpiryDate", SPFieldType.DateTime, false);
                web.Update();

                SPField expiredField = web.Fields[ExpiryDate];
                expiredField.Title = "ExpiryDate";
                expiredField.Description = "The date that the agreement has expired for which this record is pertaining to.";
                expiredField.Group = "RMToolkit";
                expiredField.Update();

                       

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }


            
        }


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
