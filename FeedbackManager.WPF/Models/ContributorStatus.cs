using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Models
{
    public static class ContributorStatus
    {
        public const string Null = "";
        public const string Student = "Student";
        public const string Staff = "Staff";
        public const string OtherStakeholder = "Other stakeholder";

        public static IEnumerable<string> ContributorStatuses
        {
            get { return new List<string>() { "", Student, Staff, OtherStakeholder }; }
        }
    }
}
