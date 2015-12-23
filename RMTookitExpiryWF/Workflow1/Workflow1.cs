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
using Microsoft.Office.RecordsManagement.RecordsRepository;
using System.Xml;
using System.IO;
using System.Text;


namespace RMTookitExpiryWF.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            string dtExpiry;
            string xmlString;
            SPDocumentLibrary MyLibrary = (SPDocumentLibrary)workflowProperties.Web.Lists[workflowProperties.ListId];
            SPListItem MyItem = MyLibrary.Items.GetItemById(workflowProperties.ItemId);
            xmlString = workflowProperties.InitiationData;
            if (xmlString.Contains("Data")) //running from button
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                {
                    reader.ReadToFollowing("expiryDate");
                    dtExpiry=reader.ReadElementContentAsString();

                }
            }
            else // getting data from initiation form (manually running wf)
            {
                dtExpiry = xmlString;
            }


            DateTime expiryDate = DateTime.Parse(dtExpiry);
            ExpireDocument(expiryDate, MyItem);
            System.Diagnostics.Debug.WriteLine(workflowProperties.InitiationData);
        }

        private bool ExpireDocument(DateTime expiryDate, SPListItem MyItem)
        {
            bool bReturn = false;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                Records.BypassLocks(MyItem, delegate(SPListItem newItem)
                {
                    newItem["ExpiryDate"] = expiryDate;
                    newItem.SystemUpdate(false);
                    bReturn = true;
                });
            });


            return (bReturn);
        }

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {

        }







    }
}
