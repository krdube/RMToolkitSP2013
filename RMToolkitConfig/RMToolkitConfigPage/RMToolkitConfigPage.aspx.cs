using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.ApplicationPages;
using System.Web.UI.WebControls;

namespace RMToolkitConfig
{
    public partial class RMTookitConfigPage : GlobalAdminPageBase
    {
    

        protected void Page_Load(object sender, EventArgs e)
        {
            // wire-up control event handlers  
            btnSubmitTop.Click += btnSubmitTop_Click;
            btnCancelTop.Click += btnCancelTop_Click;

            SPFarm farm = SPFarm.Local;
            if (!IsPostBack)
            {
                

                if (farm.Properties.ContainsKey("RMToolkitArchiveCertificateLibrary"))
                {
                    this.txtRMToolkitArchiveCertificateLibraryName.Text = farm.Properties["RMToolkitArchiveCertificateLibrary"].ToString();
                }
                else
                {
                    txtRMToolkitArchiveCertificateLibraryName.Text = "";
                }

                if (farm.Properties.ContainsKey("RMToolkitDeletionCertificateLibrary"))
                {
                    this.txtRMToolkitDeleteCertificateLibraryName.Text = farm.Properties["RMToolkitDeletionCertificateLibrary"].ToString();
                }
                else
                {
                    txtRMToolkitDeleteCertificateLibraryName.Text = "";
                }

                
                if (farm.Properties.ContainsKey("RMToolkitSite"))
                {
                    this.txtRMToolkitSiteURL.Text = farm.Properties["RMToolkitSite"].ToString();
                }
                else
                {
                    txtRMToolkitSiteURL.Text = "";
                }

                    if (farm.Properties.ContainsKey("RMToolkitGlobalColumn1"))
                    {
                        this.txtRMToolkitGlbColumn1.Text = farm.Properties["RMToolkitGlobalColumn1"].ToString();
                    
                    }
                else
                {
                    txtRMToolkitGlbColumn1.Text = "";
                }
                if (farm.Properties.ContainsKey("RMToolkitGlobalColumn2"))
                {
                    this.txtRMToolkitGlbColumn2.Text = farm.Properties["RMToolkitGlobalColumn2"].ToString();
                }
                else
                {
                    txtRMToolkitGlbColumn2.Text = "";
                }
                if (farm.Properties.ContainsKey("RMToolkitGlobalColumn3"))
                {
                    this.txtRMToolkitGlbColumn3.Text = farm.Properties["RMToolkitGlobalColumn3"].ToString();
                }
                else
                {
                    txtRMToolkitGlbColumn3.Text = "";
                }



            }
        }

      

        void btnCancelTop_Click(object sender, EventArgs e)
        {
            // go back to Application Management
            Response.Redirect("/default.aspx");
            
        }

        void btnSubmitTop_Click(object sender, EventArgs e)
        {
            // save page values and go back to Application Management
            
            SPFarm farm = SPFarm.Local;
         

            if (farm.Properties.ContainsKey("RMToolkitArchiveCertificateLibrary"))
            {
                farm.Properties["RMToolkitArchiveCertificateLibrary"] = this.txtRMToolkitArchiveCertificateLibraryName.Text;
                farm.Update();
            }
            else
            {
                farm.Properties.Add("RMToolkitArchiveCertificateLibrary", txtRMToolkitArchiveCertificateLibraryName.Text);
                farm.Update();
            }


            if (farm.Properties.ContainsKey("RMToolkitDeletionCertificateLibrary"))
            {
                farm.Properties["RMToolkitDeletionCertificateLibrary"] = this.txtRMToolkitDeleteCertificateLibraryName.Text;
                farm.Update();
            }
            else
            {
                farm.Properties.Add("RMToolkitDeletionCertificateLibrary", txtRMToolkitDeleteCertificateLibraryName.Text);
                farm.Update();
            }

            if (farm.Properties.ContainsKey("RMToolkitSite"))
            {
                farm.Properties["RMToolkitSite"] = this.txtRMToolkitSiteURL.Text;
                farm.Update();
            }
            else
            {
                farm.Properties.Add("RMToolkitSite", txtRMToolkitSiteURL.Text);
                farm.Update();
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn1"))
            {
                farm.Properties["RMToolkitGlobalColumn1"] = this.txtRMToolkitGlbColumn1.Text;
                farm.Update();
            }
            else
            {
                farm.Properties.Add("RMToolkitGlobalColumn1", this.txtRMToolkitGlbColumn1.Text);
                farm.Update();
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn2"))
            {
                farm.Properties["RMToolkitGlobalColumn2"] = this.txtRMToolkitGlbColumn2.Text;
                farm.Update();
            }
            else
            {
                farm.Properties.Add("RMToolkitGlobalColumn2", txtRMToolkitGlbColumn2.Text);
                farm.Update();
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn3"))
            {
                farm.Properties["RMToolkitGlobalColumn3"] = this.txtRMToolkitGlbColumn3.Text;
                farm.Update();
            }
            else
            {
                farm.Properties.Add("RMToolkitGlobalColumn3", txtRMToolkitGlbColumn3.Text);
                farm.Update();
            }


            Response.Redirect("/default.aspx");
        }

    }
}
