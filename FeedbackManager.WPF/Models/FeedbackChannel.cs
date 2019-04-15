using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Models
{
    public static class FeedbackChannel
    {
        public const string FeedbackForm = "Feedback form";
        public const string Email = "Email";
        public const string PhoneCall = "Phone call";

        public static IEnumerable<string> FeedbackChannels
        {
            get { return new List<string>() { "", FeedbackForm, Email, PhoneCall }; }
        }
    }
}
