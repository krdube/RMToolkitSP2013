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

        private string GetMetadataValue(SPListItem item, string columnName)
        {
            //date processing from http://brandonatkinson.blogspot.ca/2012/11/splistitemgetformattedvalue-returns.html

            string returnValue = "";
            try
            {
                if (item.Fields[columnName].Type == SPFieldType.DateTime)
                {
                    SPFieldDateTime dtField = (SPFieldDateTime)item.Fields[columnName];
                    DateTime dTime = Convert.ToDateTime(item[columnName].ToString());
                    if (dtField.DisplayFormat == SPDateTimeFieldFormatType.DateOnly)
                        returnValue = columnName + ": " + dTime.ToShortDateString();
                    else
                        returnValue = columnName + ": " + dTime.ToString();
                }
                else if (item.Fields[columnName].Type == SPFieldType.User)
                {
                    SPFieldUser field = (SPFieldUser)item.Fields[columnName];

                    if (field != null) 
                    { 
                        SPFieldUserValue fieldValue = field.GetFieldValue(item[columnName].ToString()) as SPFieldUserValue;
                        if (fieldValue != null)
                        {
                            SPUser user = fieldValue.User;
                            returnValue = columnName + ": " + user.Name;
                        }
                        else
                            returnValue = "";

                    }

                   // returnValue = field.ToString();
                   // int int_currentUser = properties.CurrentUserId;
                   // SPUser user1 = web.AllUsers.GetByID(int_currentUser);
                }
                else
                {
                    if (item.GetFormattedValue(columnName) == "")
                    {
                        returnValue = "";
                    }
                    else
                    {
                        returnValue = columnName + ": " + item.GetFormattedValue(columnName);
                    }

                }
            }
            catch
            {
                returnValue = "";
            }

            return returnValue;
        }

        

        //this worklow creates a deletion list item for approval
        private void writeToExternalList_ExecuteCode(object sender, EventArgs e)
        {
            string siteURL;
            string siteColumn1="";
            string siteColumn1Val = "";
            string siteColumn2="";
            string siteColumn2Val = "";
            string siteColumn3="";
            string siteColumn3Val = "";
            string globalColumn1="";
            string globalColumn1Val = "";
            string globalColumn2="";
            string globalColumn2Val = "";
            string globalColumn3="";
            string globalColumn3Val = "";
            siteURL = "";
           // SPWebApplication wa = SPContext.Current.Site.WebApplication;
            //SPFarm farm = SPContext.Current.Site.WebApplication.Farm;

            SPListItem MyItem = workflowProperties.Item;
            SPDocumentLibrary MyLibrary = (SPDocumentLibrary)MyItem.ParentList;
            SPWeb oWebSite = MyLibrary.ParentWeb;
            //now get metdata
            if (oWebSite.AllProperties.ContainsKey("RMToolkitSiteColumn1"))
            {
                siteColumn1 = oWebSite.AllProperties["RMToolkitSiteColumn1"].ToString();
            }

            if (oWebSite.AllProperties.ContainsKey("RMToolkitSiteColumn2"))
            {
                siteColumn2 =oWebSite.AllProperties["RMToolkitSiteColumn2"].ToString();
            }

            if (oWebSite.AllProperties.ContainsKey("RMToolkitSiteColumn3"))
            {
                siteColumn3 = oWebSite.AllProperties["RMToolkitSiteColumn3"].ToString();
            }

            SPSite mySiteCollection = oWebSite.Site;
            SPWebApplication webApplication = mySiteCollection.WebApplication;
            SPFarm farm = mySiteCollection.WebApplication.Farm;

            if (farm.Properties.ContainsKey("RMToolkitSite"))
            {
                siteURL = farm.Properties["RMToolkitSite"].ToString();
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn1"))
            {
               globalColumn1 = farm.Properties["RMToolkitGlobalColumn1"].ToString();

            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn2"))
            {
                globalColumn2 = farm.Properties["RMToolkitGlobalColumn2"].ToString();

            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn3"))
            {
                globalColumn3 = farm.Properties["RMToolkitGlobalColumn3"].ToString();

            }

            siteColumn1Val = GetMetadataValue(MyItem, siteColumn1);
            siteColumn2Val = GetMetadataValue(MyItem, siteColumn2);
            siteColumn3Val = GetMetadataValue(MyItem, siteColumn3);
            globalColumn1Val = GetMetadataValue(MyItem,globalColumn1);
            globalColumn2Val = GetMetadataValue(MyItem, globalColumn2);
            globalColumn3Val = GetMetadataValue(MyItem, globalColumn3);

            if (siteURL != "")
            {

               // SPListItem MyItem = workflowProperties.Item;
                //SPDocumentLibrary MyLibrary = (SPDocumentLibrary)MyItem.ParentList;
               
                using (SPSite RMSite = new SPSite(siteURL))
                {
                    //DisposeCheckOK
                    SPWeb RMWeb = RMSite.RootWeb;
                    SPList RMList = RMWeb.Lists["RMToolkitDeletionList"];
                    SPListItem item = RMList.Items.Add();

                    item["Title"] = MyItem.Name + "_" + "Created:" + MyItem["Created"].ToString() + "_" + MyItem.ID.ToString();
                    string url = MyItem["EncodedAbsUrl"].ToString();
                    item["DocumentURL"] = url;
                    item["DocumentLibrary"] = MyLibrary.Title;
                    item["Site"] = MyLibrary.ParentWeb.Title;
                    item["SiteCollectionURL"] = MyLibrary.ParentWeb.Site.Url;
                    item["CertificateName"] = "default";
                    item["ParentFolder"] = MyItem.Url.Substring(0, MyItem.Url.LastIndexOf('/'));
                    item["SiteColumn1"] = siteColumn1Val;
                    item["SiteColumn2"] = siteColumn2Val;
                    item["SiteColumn3"] = siteColumn3Val;
                    item["GlobalColumn1"] = globalColumn1Val;
                    item["GlobalColumn2"] = globalColumn2Val;
                    item["GlobalColumn3"] = globalColumn3Val;
                    item.Update();
                  

                    
                }
            }
        }

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {

        }
    }
}
