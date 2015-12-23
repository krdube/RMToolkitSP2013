using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;

namespace RMToolkitSiteMetadata.Layouts.RMToolkitSiteMetadata
{
    public partial class MetadataColumns : LayoutsPageBase
    {
        protected void Save(object sender, EventArgs e)
        {
            using (SPWeb spweb = SPContext.Current.Web)
            {
                //save the stuff
                string column1 = this.txtColumn1.Text;
                column1.Trim();
               // if (column1 != "")
              //  {
                    if (spweb.AllProperties.ContainsKey("RMToolkitSiteColumn1"))
                    {
                        spweb.AllProperties["RMToolkitSiteColumn1"] = this.txtColumn1.Text;
                        spweb.Update();
                    }
                    else if (column1!="")
                    {
                        spweb.AllProperties.Add("RMToolkitSiteColumn1", txtColumn1.Text);
                        spweb.Update();
                    }

               // }

                string column2 = this.txtColumn2.Text;
                column2.Trim();
              //  if (column2 != "")
              //  {
                    if (spweb.AllProperties.ContainsKey("RMToolkitSiteColumn2"))
                    {
                        spweb.AllProperties["RMToolkitSiteColumn2"] = this.txtColumn2.Text;
                        spweb.Update();
                    }
                    else if (column2!="")
                    {
                        spweb.AllProperties.Add("RMToolkitSiteColumn2", txtColumn2.Text);
                        spweb.Update();
                    }

               // }

                string column3 = this.txtColumn3.Text;
                column3.Trim();
              //  if (column3 != "")
              //  {
                    if (spweb.AllProperties.ContainsKey("RMToolkitSiteColumn3"))
                    {
                        spweb.AllProperties["RMToolkitSiteColumn3"] = this.txtColumn3.Text;
                        spweb.Update();
                    }
                    else if (column3!="")
                    {
                        spweb.AllProperties.Add("RMToolkitSiteColumn3", txtColumn3.Text);
                        spweb.Update();
                    }

              //  }


                string settingsURL = spweb.Url;
                settingsURL += "/_layouts/settings.aspx";
                Response.Redirect(settingsURL);
            }
        }

        protected void Cancel(object sender, EventArgs e)
        {
            using (SPWeb spweb = SPContext.Current.Web)
            {
                string settingsURL = spweb.Url;
                settingsURL += "/_layouts/settings.aspx";
                Response.Redirect(settingsURL);
            }
        }

       

        protected void Page_Load(object sender, EventArgs e)
        {

           
            SPWeb oWebSite = SPControl.GetContextWeb(Context);
            //this.lblSiteName.Text = oWebSite.Title;
            
            using (SPWeb spWeb = SPContext.Current.Web)
            {
          
                //string spWebTitle = spWeb.Title;
                //this.lblSiteName.Text = spWebTitle;
                if (spWeb.AllProperties.ContainsKey("RMToolkitSiteColumn1")) 
                {
                    if (!Page.IsPostBack) 
                    {
                        this.txtColumn1.Text = spWeb.AllProperties["RMToolkitSiteColumn1"].ToString();
                    }
                }

                if (spWeb.AllProperties.ContainsKey("RMToolkitSiteColumn2"))
                {
                    if (!Page.IsPostBack)
                    {
                        this.txtColumn2.Text = spWeb.AllProperties["RMToolkitSiteColumn2"].ToString();
                    }
                }

                if (spWeb.AllProperties.ContainsKey("RMToolkitSiteColumn3"))
                {
                    if (!Page.IsPostBack)
                    {
                        this.txtColumn3.Text = spWeb.AllProperties["RMToolkitSiteColumn3"].ToString();
                    }
                }

            }
            

        }
    }
}
