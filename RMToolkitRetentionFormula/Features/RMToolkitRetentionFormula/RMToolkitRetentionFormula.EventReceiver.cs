using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.Office.RecordsManagement.InformationPolicy;

namespace RMToolkitRetentionFormula.Features.RMToolkitRetentionFormula
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("9ec423d5-8a1a-4c81-9729-d17f55f1c686")]
    public class RMToolkitRetentionFormulaEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            string xmlManifest =
               "<PolicyResource xmlns=\"urn:schemas-microsoft-com:office:server:policy\"" +
               " id = \"RMToolkitRetentionFormula.TwoMinuteExpiration\"" +
               " featureId=\"Microsoft.Office.RecordsManagement.PolicyFeatures.Expiration\"" +
               " type = \"DateCalculator\">   <Name>RMToolkitTwoMinuteExpiration</Name>" +
               "<Description>Items Expire based on a filter</Description>" +
               "<AssemblyName>RMToolkitRetentionFormula, Version=1.0.0.0, Culture=neutral," +
               "PublicKeyToken=fc92a9cedfda8952</AssemblyName>" +
               "<ClassName>RMToolkitRetentionFormula.TwoMinuteExpiration</ClassName>" +
               "</PolicyResource>";
            PolicyResource.ValidateManifest(xmlManifest);
            PolicyResourceCollection.Add(xmlManifest);
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            //Delete the custom formula from the policy resources collection 
            PolicyResourceCollection.Delete("RMToolkitRetentionFormula.TwoMinuteExpiration");

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
