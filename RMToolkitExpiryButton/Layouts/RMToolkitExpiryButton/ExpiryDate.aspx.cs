using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;

namespace RMToolkitExpiryButton.Layouts.RMToolkitExpiryButton
{
    public partial class ExpiryDate : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((TextBox)(this.dtPicker.Controls[0])).Attributes.Add("readonly", "readOnly");
        }
    }
}
