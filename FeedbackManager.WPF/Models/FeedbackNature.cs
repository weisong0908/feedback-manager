using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Models
{
    public static class FeedbackNature
    {
        public const string TotalFeedbacks = "Total feedbacks";
        public const string Complaint = "Complaint";
        public const string Compliment = "Compliment";
        public const string ForInformation = "For information";

        public static IEnumerable<string> FeedbackNatures
        {
            get { return new List<string>() { "", Complaint, Compliment, ForInformation }; }
        }

        public static IEnumerable<string> FeedbackNaturesForChart
        {
            get { return new List<string>() { TotalFeedbacks, Complaint, Compliment, ForInformation }; }
        }
    }
}
