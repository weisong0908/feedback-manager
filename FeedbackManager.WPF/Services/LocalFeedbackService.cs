using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FeedbackManager.WPF.Models;

namespace FeedbackManager.WPF.Services
{
    public class LocalFeedbackService : IFeedbackService
    {
        private OleDbConnection dbConnection;
        private OleDbDataReader dataReader;
        private OleDbCommand command;

        public LocalFeedbackService()
        {
            dbConnection = new OleDbConnection((Application.Current as App).ConnectionString);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT * FROM Feedbacks", dbConnection);

            dataReader = command.ExecuteReader();

            var feedbacks = new List<Feedback>();

            while(await dataReader.ReadAsync())
            {
                feedbacks.Add(new Feedback()
                {
                    Id = int.Parse(dataReader["Id"].ToString()),
                    DateReceived = (dataReader["Date_Received"] == DBNull.Value) ? DateTime.Today : DateTime.Parse(dataReader["Date_Received"].ToString()),
                    Channel = dataReader["Channel"].ToString(),
                    Progress = dataReader["Feedback_Status"].ToString(),
                    FeedbackNature = dataReader["Feedback_Nature"].ToString(),
                    DateAcknowledged = (dataReader["Date_Acknowledgement"] == DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dataReader["Date_Acknowledgement"].ToString()),
                    ResponsibleDepartment = dataReader["Department_Responsible"].ToString(),
                    ContributorName = dataReader["Contributor_Name"].ToString(),
                    StudentId = dataReader["Student_ID"].ToString(),
                    ContributorStatus = dataReader["Contributor_Status"].ToString(),
                    CourseOrDepartment = dataReader["Course_or_Department"].ToString(),
                    Phone = dataReader["Contact_No"].ToString(),
                    Email = dataReader["Email"].ToString(),
                    Title = dataReader["Title"].ToString(),
                    FeedbackSummary = dataReader["Feedback_Summary"].ToString(),
                    ActionBy = dataReader["Action_By"].ToString(),
                    ApprovedBy = dataReader["Approved_By"].ToString(),
                    RectificationSummary = dataReader["Rectification_Summary"].ToString(),
                    DateResolved = (dataReader["Date_Resolved"] == DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dataReader["Date_Resolved"].ToString()),
                    Category = dataReader["Feedback_Code"].ToString(),
                    Remarks = dataReader["Remarks"].ToString(),
                    IsExcludedFromAnalysis = dataReader["IsExcludedFromAnalysis"].ToString(),
                    IsRemoved = dataReader["IsRemoved"].ToString()
                });
            }

            dataReader.Close();
            dbConnection.Close();

            return feedbacks;
        }
    }
}
