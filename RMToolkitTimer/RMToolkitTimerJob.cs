using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.IO;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint.Publishing;


namespace RMToolkitTimer
{
    public class RMToolkitTimerJob:SPJobDefinition
    {
        public const string JobName = "RMToolkitTimerJob"; 

        public RMToolkitTimerJob()
            : base()
        {
        }

        //public RMToolkitTimerJob(string jobName, SPService service, 
        //       SPServer server, SPJobLockType lockType)
        //       : base(jobName, service, server, lockType)
        //{
        //    this.Title = "RMToolkitTimerJob";
        //}

        public RMToolkitTimerJob(SPWebApplication webapp)
            : base(JobName, webapp, null, SPJobLockType.Job)
        {
           
        }

        private string GetDeletionQueryString()
        {
             StringBuilder queryXML = new StringBuilder();
           // queryXML.Append("<Query>");
            queryXML.Append("<Where>");
            queryXML.Append("<And>");
            queryXML.Append("<Eq>");
            queryXML.Append("<FieldRef Name='ApprovedForDeletion' />");
            queryXML.Append("<Value Type='Integer'>1</Value>");
            queryXML.Append("</Eq>");
            queryXML.Append("<And>");
            queryXML.Append("<Eq>");
            queryXML.Append("<FieldRef Name='RecordDeleted' />");
            queryXML.Append("<Value Type='Integer'>0</Value>");
            queryXML.Append("</Eq>");
            queryXML.Append("<Eq>");
            queryXML.Append("<FieldRef Name='RetainRecord' />");
            queryXML.Append("<Value Type='Integer'>0</Value>");
            queryXML.Append("</Eq>");
            queryXML.Append("</And>");
            queryXML.Append("</And>");
            queryXML.Append("</Where>");
           // queryXML.Append("</Query>");
    
             //Assigning custom CAML query to query object            
            return (queryXML.ToString());

        }

        private string GetArchiveQueryString()
        {
            StringBuilder queryXML = new StringBuilder();
            // queryXML.Append("<Query>");
            queryXML.Append("<Where>");
            queryXML.Append("<And>");
            queryXML.Append("<Eq>");
            queryXML.Append("<FieldRef Name='ApprovedForArchive' />");
            queryXML.Append("<Value Type='Integer'>1</Value>");
            queryXML.Append("</Eq>");
            queryXML.Append("<And>");
            queryXML.Append("<Eq>");
            queryXML.Append("<FieldRef Name='Archived' />");
            queryXML.Append("<Value Type='Integer'>0</Value>");
            queryXML.Append("</Eq>");
            queryXML.Append("<Eq>");
            queryXML.Append("<FieldRef Name='RetainRecord' />");
            queryXML.Append("<Value Type='Integer'>0</Value>");
            queryXML.Append("</Eq>");
            queryXML.Append("</And>");
            queryXML.Append("</And>");
            queryXML.Append("</Where>");
            // queryXML.Append("</Query>");

            //Assigning custom CAML query to query object            
            return (queryXML.ToString());

        }
        private void ProcessDeletionItems(SPWeb web,
                                            SPList deletionList, 
                                            DateTime dateAsDate, 
                                            string dateString, 
                                            string defaultCertificateName,
                                            string deletionCertificateLibraryName)
        {
            //query deletion list
            List<SPListItem> deletionListItems = new List<SPListItem>();
            List<SPListItem> actuallydeletedListItems = new List<SPListItem>();
            List<string> certificates = new List<string>();
            SPQuery deletionQuery = new SPQuery();
            deletionQuery.Query = GetDeletionQueryString();

            SPListItemCollection lc = deletionList.GetItems(deletionQuery);
            //Executing custom CAML query and converting results to list            
            deletionListItems = deletionList
                .GetItems(deletionQuery)
                .Cast<SPListItem>()
                .ToList();

            //iterate through list items

            foreach (var listItem in deletionListItems)
            {
                SPFieldUrlValue value = new SPFieldUrlValue(listItem["DocumentURL"].ToString());
                string docURL = value.Url;
                if (UndeclareAndDeleteDocument(docURL))
                {
                    listItem["RecordDeleted"] = 1;
                    listItem["DeletionDate"] = dateAsDate;
                    listItem.Update();
                    actuallydeletedListItems.Add(listItem);
                    Log("RMToolkit", TraceSeverity.High, EventSeverity.Information, "Deleted file: " + docURL);
                }
                else //couldn't delete for some reason
                {
                    Log("RMToolkit", TraceSeverity.High, EventSeverity.Error, "Couldn't delete file: " + docURL); 
                }

            }
            if (deletionListItems.Count > 0)
            {
                Log("RMToolkit", TraceSeverity.High, EventSeverity.Information, "Creating Certificates");
                certificates = GenerateCertificateList(dateString, actuallydeletedListItems, ref defaultCertificateName);
                SaveTextFiles(DispositionType.Deletion, dateString, web, certificates, actuallydeletedListItems, deletionCertificateLibraryName, defaultCertificateName);
            }
            else
            {
                Log("RMToolkit", TraceSeverity.High, EventSeverity.Information, "No Files Ready for Destruction");
            }
        }

        public bool ArchiveDocument(string docURL)
        {
            bool bReturn = false;
            using (SPSite site = new SPSite(docURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPFile file = web.GetFile(docURL);
                    SPListItem MyItem = file.Item;
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        Records.BypassLocks(MyItem, delegate(SPListItem newItem)
                        {
                            newItem["Archived"] = 1;
                            newItem.SystemUpdate(false);
                            bReturn = true;
                        });
                    });
                }
            }
            return (bReturn);
        }
                     

        private void ProcessArchiveItems(SPWeb web,
                                            SPList archiveList,
                                            DateTime dateAsDate,
                                            string dateString,
                                            string defaultCertificateName,
                                            string archiveCertificateLibraryName)
        {
            //query deletion list
            List<SPListItem> archiveListItems = new List<SPListItem>();
            List<SPListItem> actuallyArchivedListItems = new List<SPListItem>();
            List<string> certificates = new List<string>();
            SPQuery archiveQuery = new SPQuery();
            archiveQuery.Query = GetArchiveQueryString();

            SPListItemCollection lc = archiveList.GetItems(archiveQuery);
            //Executing custom CAML query and converting results to list            
            archiveListItems = archiveList
                .GetItems(archiveQuery)
                .Cast<SPListItem>()
                .ToList();

            //iterate through list items

            foreach (var listItem in archiveListItems)
            {
                SPFieldUrlValue value = new SPFieldUrlValue(listItem["DocumentURL"].ToString());
                string docURL = value.Url;
                if (ArchiveDocument(docURL))
                {
                    listItem["Archived"] = 1;
                    listItem["ArchiveDate"] = dateAsDate;
                    Log("about to archive list item" + listItem["Title"], TraceSeverity.High, EventSeverity.Information, "Just starting execute procedure");
                    listItem.Update();
                    actuallyArchivedListItems.Add(listItem);
                }
                else //couldn't delete for some reason
                {
                    //do nothing
                }

            }
            if (actuallyArchivedListItems.Count > 0)
            {
                Log("RMToolkit", TraceSeverity.High, EventSeverity.Information, "Creating Archive Certificates");
                certificates = GenerateCertificateList(dateString, actuallyArchivedListItems, ref defaultCertificateName);
                SaveTextFiles(DispositionType.Archive, dateString, web, certificates, actuallyArchivedListItems, archiveCertificateLibraryName, defaultCertificateName);
            }
            else
            {
                Log("RMToolkit", TraceSeverity.High, EventSeverity.Information, "No Files Ready for Destruction");
            }
        }

        public enum DispositionType { Archive, Deletion };

        public override void Execute(Guid targetInstanceId)
        {

          
            string deletionCertificateLibraryName="";
            string archiveCertificateLibraryName="";
            string toolkitURL="";
            string defaultCertificateName = "";
            string deletionListName = "RMToolkitDeletionList";
            string archiveListName = "RMToolkitArchiveList";
            string dateString = string.Format("{0:yyyy-MM-dd_hh-mm}", DateTime.Now);
            string globalColumn1="";
            string globalColumn2="";
            string globalColumn3="";
            DateTime dateAsDate=DateTime.Now;
            //SPFarm farm = SPFarm.Local;

            //log to file
            Log("RMToolkitTimerJob", TraceSeverity.High, EventSeverity.Error, "Just starting execute procedure");
            //steps
            //get config items for farm
            
            //need deletion list, archive list, deletion certificate library, archive certificate library
            GetConfigValues(ref toolkitURL, ref deletionCertificateLibraryName, ref archiveCertificateLibraryName, ref globalColumn1, ref globalColumn2, ref globalColumn3);
          
            //query list for items which are approved for deletion but are not deleted
            using (SPSite site=new SPSite(toolkitURL))
            //toolkitURL should be like http://localhost/sites/sitecollection/site
            {
                using(SPWeb web=site.OpenWeb())
                {
                    SPList deletionList = web.Lists[deletionListName];
                    SPList archiveList = web.Lists[archiveListName];

                    ProcessDeletionItems(web,deletionList, dateAsDate, dateString, defaultCertificateName, deletionCertificateLibraryName);
                    ProcessArchiveItems(web, archiveList, dateAsDate, dateString, defaultCertificateName, archiveCertificateLibraryName);
                }

            } 
                   
           ////iterate though list items undeclaring and deleting them and adding them to in memory array

            //when done write this array to PDF and sae PDF to deletionCertificateLibrary
        }

        public bool UndeclareAndDeleteDocument(string docURL)
        {
            bool bReturn = false;
            try
            {
                using (SPSite site = new SPSite(docURL))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPFile file = web.GetFile(docURL);
                        SPListItem MyItem = file.Item;
                       SPSecurity.RunWithElevatedPrivileges(delegate()
                       {
                            try
                            {
                                if (Records.IsRecord(MyItem))
                                {
                                    Records.UndeclareItemAsRecord(MyItem);
                                    MyItem.Delete();
                                    bReturn = true;
                                }
                                else
                                {
                                    MyItem.Delete();
                                    bReturn = true;
                                }
                            }
                            catch
                            {
                                bReturn = false;
                            }

                        });

                    }
                }
            }
            catch
            {
                bReturn = false;
            }

            return bReturn;

        }

        //outputs certificates to specified library
        public void SaveTextFiles(DispositionType dispType,string dateString, SPWeb web, List<string> certificates, List<SPListItem> deletionListItems, string deletionCertificateLibraryName, string defaultCertName)
        {
           // string dateString = string.Format("{0:yyyy-MM-dd_hh-mm}", DateTime.Now);
            List<MemoryStream> mstreams=new List<MemoryStream>();
            //List<StreamWriter> streamwriters;
           // Stream stream = null;
            SPList sourceListOBj = web.Lists[deletionCertificateLibraryName];
           // StreamWriter sw = new StreamWriter(mstream);
            foreach (var certificate in certificates)
            {
                MemoryStream mstream=new MemoryStream();
                StreamWriter sw=new StreamWriter(mstream);
                if (dispType == DispositionType.Deletion)
                {
                    sw.WriteLine("Certificate of Destruction");
                }
                else
                {
                    sw.WriteLine("Certificate of Archive");
                }
                sw.WriteLine("Generated: " + dateString);
                sw.WriteLine("Certificate: " + certificate);
                sw.WriteLine("Title,DocumentURL,Created,SiteCollectionURL,DocumentLibrary,ParentFolder, Approver, Global Column1, Global Column2, Global Column3, Site Column1, Site Column 2, Site Column 3");
                foreach (var listItem in deletionListItems)
                {
                  
                    if (listItem["CertificateName"].ToString()==certificate)
                    {
                        string lineitem;
                        lineitem = listItem["Title"].ToString();
                        lineitem = String.Concat(lineitem, ",");
                        SPFieldUrlValue value= new SPFieldUrlValue(listItem["DocumentURL"].ToString());
                        lineitem = String.Concat(lineitem, value.Url);
                        lineitem = String.Concat(lineitem, ",");
                        lineitem = String.Concat(lineitem, listItem["Created"].ToString());
                        lineitem = String.Concat(lineitem, ",");
                        SPFieldUrlValue value1 = new SPFieldUrlValue(listItem["SiteCollectionURL"].ToString());
                        lineitem = String.Concat(lineitem, value1.Url);
                        lineitem = String.Concat(lineitem, ",");
                        lineitem = String.Concat(lineitem, listItem["DocumentLibrary"].ToString());
                        lineitem = String.Concat(lineitem, ",");
                        lineitem = String.Concat(lineitem, listItem["ParentFolder"].ToString());


                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["Approver"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["Approver"].ToString());
                        }
                        
                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["GlobalColumn1"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["GlobalColumn1"].ToString());
                        }
                      
                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["GlobalColumn2"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["GlobalColumn2"].ToString());
                        }

                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["GlobalColumn3"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["GlobalColumn3"].ToString());
                        }

                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["SiteColumn1"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["SiteColumn1"].ToString());
                        }

                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["SiteColumn2"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["SiteColumn2"].ToString());
                        }

                        lineitem = String.Concat(lineitem, ",");
                        if (listItem["SiteColumn3"] != null)
                        {
                            lineitem = String.Concat(lineitem, listItem["SiteColumn3"].ToString());
                        }

                       
                        sw.WriteLine(lineitem);
                    }    
                }
                sw.Flush();
                mstreams.Add(mstream);
            }
            //sw.WriteLine();

            //now write array of memorystreams to library
            web.AllowUnsafeUpdates = true;
            int i=0;
            foreach (var memorystream in mstreams)
            {
                byte[] contents = new byte[memorystream.Length];
                memorystream.Read(contents, 0, (int)memorystream.Length);
                string strFileName = certificates[i];
                strFileName = String.Concat(strFileName, ".csv");
                SPFile sourceItem=web.Files.Add(web.Url+"/" + sourceListOBj.RootFolder.ToString() + "/" + strFileName,memorystream,false);
                i = i + 1;
            }


        }

     

        //this function concats the date to the certificate name to ensure its unique
        //generates alist of certificates (there will only be one if the name is left as default)
        public List<string> GenerateCertificateList(string dateString, List<SPListItem> ListItems, ref string defaultCertName)
        {

            List<string> certList=new List<string>();
            defaultCertName = string.Format("{0:yyyy-MM-dd_hh-mm}_DefaultCertificate", DateTime.Now);
            string datepart = String.Concat(dateString, "_");
            foreach (var listItem in ListItems)
            {
                    string certName=listItem["CertificateName"].ToString();
                    if (certName == "default") //default certificate set it to default name above
                    {
                        certName = defaultCertName;
                        listItem["CertificateName"] = certName;
                        listItem.Update();
                    }
                    else
                    {
                        certName = String.Concat(datepart, certName);
                        listItem["CertificateName"] = certName;
                        listItem.Update();
                    }

                    if (!certList.Contains(certName))
                    {
                        certList.Add(certName);
                    }
            }
            return certList;


        
        }


        //linq  way i decided not to use it because of possible performance issues
        //im just goint to use a caml query
        //// Get DataContext from page context
        //MyEntitiesDataContext data = new MyEntitiesDataContext(deletionListURL);

        //// Get the SharePoint list
        //EntityList<Customer> Customers = data.GetList<Customer>("Customers");

        //// Query for customers from London
        //var recordsForDeletion = from record in data.RMToolkitDeletionList 
        //                      where record.ApprovedForDeletion=true  && record.RecordDeleted==false

        //                      select record;

        //foreach (var londonCust in londonCustomers)
        //{
        //    Console.Writeline("id = {0}, City = {1}",
        //                      londonCust.CustomerId,
        //                      londonCust.City);
        //}

        /// <summary>
        /// retreives farm configuration values for timer job
        /// </summary>
        /// <param name="deletionListURL"></param>
        /// <param name="archiveListURL"></param>
        /// <param name="deletionCertificateLibraryURL"></param>
        /// <param name="archiveCertificateLibraryURL"></param>
        private void GetConfigValues(ref string toolkitURL, 
            ref string deletionCertificateLibraryName, ref string archiveCertificateLibraryName,
            ref string globalColumn1, ref string globalColumn2, ref string globalColumn3)
        {
            SPFarm farm = SPFarm.Local;
          
            if (farm.Properties.ContainsKey("RMToolkitArchiveCertificateLibrary"))
            {
                archiveCertificateLibraryName = farm.Properties["RMToolkitArchiveCertificateLibrary"].ToString();
            }
            else
            {
                archiveCertificateLibraryName = "";
            }

            if (farm.Properties.ContainsKey("RMToolkitDeletionCertificateLibrary"))
            {
                deletionCertificateLibraryName = farm.Properties["RMToolkitDeletionCertificateLibrary"].ToString();
            }
            else
            {
                deletionCertificateLibraryName = "";
            }

            if (farm.Properties.ContainsKey("RMToolkitSite"))
            {
                toolkitURL = farm.Properties["RMToolkitSite"].ToString();
            }
            else
            {
                toolkitURL = "";
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn1"))
            {
                globalColumn1 = farm.Properties["RMToolkitGlobalColumn1"].ToString();
            }
            else
            {
                globalColumn1 = "";
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn2"))
            {
                globalColumn2 = farm.Properties["RMToolkitGlobalColumn2"].ToString();
            }
            else
            {
                globalColumn2 = "";
            }

            if (farm.Properties.ContainsKey("RMToolkitGlobalColumn3"))
            {
                globalColumn3 = farm.Properties["RMToolkitGlobalColumn3"].ToString();
            }
            else
            {
                globalColumn3 = "";
            }


        }
        

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



    
    }
}
