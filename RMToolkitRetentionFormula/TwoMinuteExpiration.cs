using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.RecordsManagement.PolicyFeatures;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using Microsoft.SharePoint.Administration;

namespace RMToolkitRetentionFormula
{
    public class TwoMinuteExpiration : IExpirationFormula
    {
        public DateTime? ComputeExpireDate(SPListItem item, System.Xml.XmlNode parametersData)
        {
           Log("CustomRetentionPolicy", TraceSeverity.High, EventSeverity.Error, "RMToolkit: Two Minute Expiration entering expiry date calculation");
           DateTime expiryDate;
           DateTime recordsDeclarationDate;
           System.TimeSpan duration=new System.TimeSpan(0,2,0);
           System.TimeSpan thousandyears=new System.TimeSpan(3650000,0,0,0);
           
           if (Records.IsRecord(item))
           {
               recordsDeclarationDate = (DateTime)Records.RecordDeclarationDate(item);
               expiryDate = recordsDeclarationDate.Add(duration);
               Log("CustomRetentionPolicy", TraceSeverity.High, EventSeverity.Error, "RMToolkit: Two Minute Expiration is a record, expiry date calculated to:" + expiryDate.ToString());
           }
           else
           {
               
               expiryDate=DateTime.Now.Add(duration); //it should never expire
               Log("CustomRetentionPolicy", TraceSeverity.High, EventSeverity.Error, "RMToolkit Two Minute Expiration not a record so expiry is not applicable" + expiryDate.ToString());
           }

          
           return expiryDate;
 
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
