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

namespace RMToolkitDeleteRecord.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void undeclareRecord_ExecuteCode(object sender, EventArgs e)
        {
            SPDocumentLibrary MyLibrary = (SPDocumentLibrary)workflowProperties.Web.Lists[workflowProperties.ListId];
            SPListItem MyItem = MyLibrary.Items.GetItemById(workflowProperties.ItemId);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                if (Records.IsRecord(MyItem))
                {
                    Records.UndeclareItemAsRecord(MyItem);
                }
            });
            LogComment("Record " + MyItem.Name.ToString() + " undeclared");
        }

        private void deleteRecord_ExecuteCode(object sender, EventArgs e)
        {
            SPDocumentLibrary MyLibrary = (SPDocumentLibrary)workflowProperties.Web.Lists[workflowProperties.ListId];
            SPListItem MyItem = MyLibrary.Items.GetItemById(workflowProperties.ItemId);
            MyItem.Delete();
            LogComment("Record " + MyItem.Name.ToString() + " deleted");
        }

        private void LogComment(string logMessage)
        {
            SPWorkflow.CreateHistoryEvent(workflowProperties.Web, this.WorkflowInstanceId, 0, workflowProperties.Web.CurrentUser, new TimeSpan(), "Update", logMessage, string.Empty);
        }


    }
}
