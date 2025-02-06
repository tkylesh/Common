using CommonAPICommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Services
{
    public class EmailService
    {
        private static readonly log2sql log = new log2sql(nameof(EmailService));
        public void SendEmail(string _isoType, int quoteID, string errorMessage, string transactionID)
        {
            // this is sending an email when Lexis Nexis communicatin has an error
            var emailTimeInterval = Convert.ToInt16(ConfigurationManager.AppSettings["EmailTimeInterval"]);

            var message = new StringBuilder();
            message.AppendLine(_isoType + " Error: ");
            message.AppendLine("There has been a communication failure with the Lexis Nexis Auto Data Prefill call. ");
            message.AppendLine("Error Message: " + errorMessage);
            message.AppendLine("Check RTRPOLICYLOG and BBDB..Prefill_CV_Clue_ErrorLog for Stack Trace");
            message.AppendLine("TransactionID: " + transactionID);
            message.AppendLine("QuoteID: " + quoteID);

            var emailMessage = new EmailMessage();
            emailMessage.EmailTo = ConfigurationManager.AppSettings["OutageEmail_ToAddress"];
            emailMessage.EmailFrom = ConfigurationManager.AppSettings["OutageEmail_FromAddress"];
            emailMessage.EmailSubject = "Lexis Nexis " + _isoType + " Outage Error";
            emailMessage.EmailBody = message.ToString();

            var emailSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(emailMessage);
            var wc = new WebClient();
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            wc.Headers.Clear();
            wc.Headers.Add("Content-Type", "application/json");
            var baseuri = ConfigurationManager.AppSettings["BaseEmailURI"];
            var emailApi = ConfigurationManager.AppSettings["OutageApiAddress"];
            var request = baseuri + emailApi;

            try
            {
                wc.UploadString(request, emailSerialized);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " Error sending email for Lexis Nexis " + _isoType + " error.", ex);
            }
        }

        public partial class EmailMessage
        {
            public string EmailTo { get; set; }
            public string EmailFrom { get; set; }
            public string EmailSubject { get; set; }
            public string EmailBody { get; set; }
        }
    }
}
