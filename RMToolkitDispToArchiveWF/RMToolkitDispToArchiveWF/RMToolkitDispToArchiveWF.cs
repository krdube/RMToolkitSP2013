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

namespace RMToolkitDispToArchiveWF.RMToolkitDispToArchiveWF
{
    public sealed partial class RMToolkitDispToArchiveWF : SequentialWorkflowActivity
    {
        public RMToolkitDispToArchiveWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void Log(string source, TraceSeverity traceSeverity, EventSeverity eventSeverity, string logMessage)
        {
            try
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(source, traceSeverity, eventSeverity), traceSeverity, logMessage, null);
            }
            catch (Exception)
            {
                // maybe write to Event Log?
            }
        }

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

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            {
                Log("Archive WF", TraceSeverity.High, EventSeverity.Information, "Starting WF");

                string siteURL;
                string siteColumn1 = "";
                string siteColumn1Val = "";
                string siteColumn2 = "";
                string siteColumn2Val = "";
                string siteColumn3 = "";
                string siteColumn3Val = "";
                string globalColumn1 = "";
                string globalColumn1Val = "";
                string globalColumn2 = "";
                string globalColumn2Val = "";
                string globalColumn3 = "";
                string globalColumn3Val = "";
                siteURL = "";

                SPListItem MyItem = workflowProperties.Item;
                SPDocumentLibrary MyLibrary = (SPDocumentLibrary)MyItem.ParentList;
                SPWeb oWebSite = MyLibrary.ParentWeb;
                SPSite mySiteCollection = oWebSite.Site;
                SPWebApplication webApplication = mySiteCollection.WebApplication;
                SPFarm farm = mySiteCollection.WebApplication.Farm;


                // Log("Archive WF", TraceSeverity.High, EventSeverity.Information, "got farm reference");
                if (farm.Properties.ContainsKey("RMToolkitSite"))
                {
                    siteURL = farm.Properties["RMToolkitSite"].ToString();
                }
                //Log("Archive WF", TraceSeverity.High, EventSeverity.Information, "got site url");


                //now get metdata
                if (oWebSite.AllProperties.ContainsKey("RMToolkitSiteColumn1"))
                {
                    siteColumn1 = oWebSite.AllProperties["RMToolkitSiteColumn1"].ToString();
                }

                if (oWebSite.AllProperties.ContainsKey("RMToolkitSiteColumn2"))
                {
                    siteColumn2 = oWebSite.AllProperties["RMToolkitSiteColumn2"].ToString();
                }

                if (oWebSite.AllProperties.ContainsKey("RMToolkitSiteColumn3"))
                {
                    siteColumn3 = oWebSite.AllProperties["RMToolkitSiteColumn3"].ToString();
                }



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
                globalColumn1Val = GetMetadataValue(MyItem, globalColumn1);
                globalColumn2Val = GetMetadataValue(MyItem, globalColumn2);
                globalColumn3Val = GetMetadataValue(MyItem, globalColumn3);

                if (siteURL != "")
                {



                    // Log("Archive WF", TraceSeverity.High, EventSeverity.Information, "got item");
                    using (SPSite RMSite = new SPSite(siteURL))
                    {
                        //DisposeCheckOK
                        SPWeb RMWeb = RMSite.RootWeb;
                        //note this is a hardcoded list name
                        SPList RMList = RMWeb.Lists["RMToolkitArchiveList"];
                        SPListItem item = RMList.Items.Add();
                        item["Title"] = MyItem.Name + "_" + "Created:" + MyItem["Created"].ToString() + "_" + MyItem.ID.ToString();
                        //item["Title"] = MyItem["Created"].ToString() + "_" + MyItem.Name;
                        string url = MyItem["EncodedAbsUrl"].ToString();
                        item["DocumentURL"] = url;
                        item["DocumentLibrary"] = MyLibrary.Title;
                        item["Site"] = MyLibrary.ParentWeb.Title;
                        item["SiteCollectionURL"] = MyLibrary.ParentWeb.Site.Url;
                        //certificate name is default user can change this if they want it in a different certificate, it will get postpended by the date by the timer job
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
        }
    }
}
