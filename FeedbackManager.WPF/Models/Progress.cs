using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Models
{
    public static class Progress
    {
        public const string Null = "";
        public const string Received = "Received";
        public const string Acknowledged = "Acknowledged";
        public const string ActionApproved = "Action approved";
        public const string ClosurePending = "Closure pending";
        public const string Closed = "Closed";
    }
}
