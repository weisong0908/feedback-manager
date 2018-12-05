using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace FeedbackManager.WPF.Helpers
{
    public static class EmailHelper
    {
        public static void Send(string recipientAddress, string subject=null)
        {
            Application outlook = new Application();
            var emailTemplate = @"S:\Quality Assurance\#QA & COMPLIANCE Dept Functions#\Feedback Management\Feedback Register\Feedback Acknowledgement Template\Curtin Singapore Feedback Acknowledgement.oft";
            MailItem mailItem = (MailItem)outlook.CreateItemFromTemplate(emailTemplate);
            mailItem.To = recipientAddress;

            Accounts accounts = outlook.Session.Accounts;
            if (accounts != null)
            {
                foreach (Account account in accounts)
                {
                    if (account.SmtpAddress.ToLower() == "feedback@curtin.edu.sg")
                        mailItem.Sender = account.CurrentUser.AddressEntry;
                }
                
                if(!string.IsNullOrEmpty(subject))
                {
                    mailItem.Subject += $" - {subject}";
                }

                mailItem.Display(null);
            }
        }
    }
}
