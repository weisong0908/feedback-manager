using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public DateTime DateReceived { get; set; }
        public string ContributorName { get; set; }
        public string StudentId { get; set; }
        public string ContributorStatus { get; set; }
        public string CourseOrDepartment { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FeedbackNature { get; set; }
        public string Channel { get; set; }
        public string Title { get; set; }
        public string FeedbackSummary { get; set; }
        public DateTime? DateAcknowledged { get; set; }
        public string ResponsibleDepartment { get; set; }
        public string RectificationSummary { get; set; }
        public DateTime? DateResolved { get; set; }
        public string Progress { get; set; }
        public string Category { get; set; }
        public string Remarks { get; set; }
        public string ActionBy { get; set; }
        public string ApprovedBy { get; set; }
        public string IsExcludedFromAnalysis { get; set; }
        public string IsRemoved { get; set; }
    }
}
