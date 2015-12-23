using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using Microsoft.SharePoint.Administration;

namespace RMToolkitDispositionToDeletionListWF.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        
        private void writeToExternalList_ExecuteCode(object sender, EventArgs e)
        {
            string siteURL;
            siteURL = "";
            SPWebApplication wa = SPContext.Current.Site.WebApplication;
            SPFarm farm = SPContext.Current.Site.WebApplication.Farm;




            if (farm.Properties.ContainsKey("RMToolkitSite"))
            {
                siteURL = farm.Properties["RMToolkitSite"].ToString();
            }


            if (siteURL != "")
            {

                //SPDocumentLibrary MyLibrary = (SPDocumentLibrary)workflowProperties.Web.Lists[workflowProperties.ListId];
                // SPListItem MyItem = MyLibrary.Items.GetItemById(workflowProperties.ItemId);
                SPListItem MyItem = workflowProperties.Item;
                SPDocumentLibrary MyLibrary = (SPDocumentLibrary)MyItem.ParentList;
                //SPWebApplication currentWebApp = SPContext.Current.Site.WebApplication;
                // SPSite RMSite = currentWebApp.Sites["sites/RecordsManagement"];
                //SPSite RMSite = currentWebApp.Sites["demo/RecordsManagement"];


                SPSite RMSite = new SPSite(siteURL);
                SPWeb RMWeb = RMSite.RootWeb;
                SPList RMList = RMWeb.Lists["RMToolkitDeletionList"];
                SPListItem item = RMList.Items.Add();

                item["Title"] = MyItem["Created"].ToString()+ "_" + MyItem.Name;
                string url = MyItem["EncodedAbsUrl"].ToString();
                item["DocumentURL"] = url;
                item["DocumentLibrary"] = MyLibrary.Title;
                item["Site"] = MyLibrary.ParentWeb.Title;
                item["SiteCollectionURL"] = MyLibrary.ParentWeb.Site.Url;
                item["CertificateName"] = "default";
                item["ParentFolder"] = MyItem.Url.Substring(0, MyItem.Url.LastIndexOf('/'));
                item.Update();
            }
        }
    }
}
