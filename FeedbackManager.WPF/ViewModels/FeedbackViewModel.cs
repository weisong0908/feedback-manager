using FeedbackManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        private DateTime _dateReceived;
        public DateTime DateReceived
        {
            get { return _dateReceived; }
            set { SetValue(ref _dateReceived, value); }
        }

        private string _channel;
        public string Channel
        {
            get { return _channel; }
            set { SetValue(ref _channel, value); }
        }

        private string _progress;
        public string Progress
        {
            get { return _progress; }
            set { SetValue(ref _progress, value); }
        }

        private string _feedbackNature;
        public string FeedbackNature
        {
            get { return _feedbackNature; }
            set { SetValue(ref _feedbackNature, value); }
        }

        private DateTime? _dateAcknowledged = DateTime.MinValue;
        public DateTime? DateAcknowledged
        {
            get { return _dateAcknowledged; }
            set { SetValue(ref _dateAcknowledged, value); }
        }

        private string _responsibleDepartment;
        public string ResponsibleDepartment
        {
            get { return _responsibleDepartment; }
            set { SetValue(ref _responsibleDepartment, value); }
        }

        private string _contributorName = string.Empty;
        public string ContributorName
        {
            get { return _contributorName; }
            set { SetValue(ref _contributorName, value); }
        }

        private string _studentId = string.Empty;
        public string StudentId
        {
            get { return _studentId; }
            set { SetValue(ref _studentId, value); }
        }

        private string _contributorStatus;
        public string ContributorStatus
        {
            get { return _contributorStatus; }
            set { SetValue(ref _contributorStatus, value); }
        }

        private string _affiliation;
        public string Affiliation
        {
            get { return _affiliation; }
            set { SetValue(ref _affiliation, value); }
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get { return _phone; }
            set { SetValue(ref _phone, value); }
        }

        private string _email = string.Empty;
        public string Email
        {
            get { return _email; }
            set { SetValue(ref _email, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        private string _feedbackSummary = string.Empty;
        public string FeedbackSummary
        {
            get { return _feedbackSummary; }
            set { SetValue(ref _feedbackSummary, value); }
        }

        private string _actionBy = string.Empty;
        public string ActionBy
        {
            get { return _actionBy; }
            set { SetValue(ref _actionBy, value); }
        }

        private string _approvedBy = string.Empty;
        public string ApprovedBy
        {
            get { return _approvedBy; }
            set { SetValue(ref _approvedBy, value); }
        }

        private string _rectificationSummary = string.Empty;
        public string RectificationSummary
        {
            get { return _rectificationSummary; }
            set { SetValue(ref _rectificationSummary, value); }
        }

        private DateTime? _dateResolved = DateTime.MinValue;
        public DateTime? DateResolved
        {
            get { return _dateResolved; }
            set { SetValue(ref _dateResolved, value); }
        }

        private string _category = string.Empty;
        public string Category
        {
            get { return _category; }
            set { SetValue(ref _category, value); }
        }

        private bool _isExcludedFromAnalysis = false;
        public bool IsExcludedFromAnalysis
        {
            get { return _isExcludedFromAnalysis; }
            set { SetValue(ref _isExcludedFromAnalysis, value); }
        }

        private string _remarks = string.Empty;
        public string Remarks
        {
            get { return _remarks; }
            set { SetValue(ref _remarks, value); }
        }
    }
}
