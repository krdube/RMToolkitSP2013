using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.RecordsManagement.PolicyFeatures;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint.Administration;

namespace CustomRetentionFormula
{
    public class RMToolkit1MinuteExpiration:IExpirationFormula
    {
       public Nullable<DateTime> ComputeExpireDate(SPListItem item, System.Xml.XmlNode parametersData)
        {
            //Log("RMToolkitCustomRetentionPolicy", TraceSeverity.High, EventSeverity.Error, "RMToolkitCustomRetentionPolicy entering expiry date calculation");
           //DateTime expiryDate;
          
          // DateTime recordsDeclarationDate;
          // System.TimeSpan duration=new System.TimeSpan(0,1,0);
          // expiryDate = DateTime.Now.Add(duration);
           //System.TimeSpan thousandyears=new System.TimeSpan(3650000,0,0,0);
           
           //if (Records.IsRecord(item))
           //{
           //    recordsDeclarationDate = (DateTime)Records.RecordDeclarationDate(item);
           //    expiryDate = recordsDeclarationDate.Add(duration);
           //    Log("RMToolkitCustomRetentionPolicy", TraceSeverity.High, EventSeverity.Error, "RMToolkitCustomRetentionPolicy is a record, expiry date calculated to:" + expiryDate.ToString());
           //}
           //else
           //{
               
           //    expiryDate=DateTime.Now.Add(duration); //it should never expire
           //    Log("RMToolkitCustomRetentionPolicy", TraceSeverity.High, EventSeverity.Error, "RMToolkitCustomRetentionPolicy not a record, date calculated to:" + expiryDate.ToString());
           //}

          
           return System.DateTime.Now;
          
           


           //return DateTime.Now.Subtract(duration);
           
           //    if (item["MyCustomColumn"].ToString().Equals("Some Value"))
        //    {
        //        return DateTime.Now;
        //    }
        //    else
        //        return null;
        }

        //private void Log(string source, TraceSeverity traceSeverity, EventSeverity eventSeverity, string logMessage)
        //{
        //    try
        //    {
        //        SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(source, traceSeverity, eventSeverity), traceSeverity, logMessage, null);
        //    }
        //    catch (Exception)
        //    {
        //        // maybe write to Event Log?
        //    }
        //}
    }
}
