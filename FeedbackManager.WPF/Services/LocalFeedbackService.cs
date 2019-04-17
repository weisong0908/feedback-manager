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

            while (await dataReader.ReadAsync())
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
                    Affiliation = dataReader["Course_or_Department"].ToString(),
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

            return feedbacks.Where(f => f.IsRemoved.ToLower() != "yes");
        }

        public async Task<int> GetFeedbackID(Feedback feedback)
        {
            dbConnection.Open();
            command = new OleDbCommand("SELECT Id FROM Feedbacks WHERE Date_Received=@DateReceived AND Contributor_Name=@ContributorName", dbConnection);

            command.Parameters.AddWithValue("@DateReceived", feedback.DateReceived.ToShortDateString());
            command.Parameters.AddWithValue("@ContributorName", feedback.ContributorName);

            dataReader = command.ExecuteReader();

            int id = 0;
            while (await dataReader.ReadAsync())
            {
                id = int.Parse(dataReader["Id"].ToString());
            }

            dataReader.Close();
            dbConnection.Close();

            return id;
        }

        public async Task<Feedback> AddNewFeedback(Feedback feedback)
        {
            string sql = "INSERT INTO Feedbacks " +
                "(Date_Received, Contributor_Name, Student_ID, Contributor_Status, Course_or_Department, Contact_No, " +
                "Email, Feedback_Nature, Channel, Title, Feedback_Summary, Date_Acknowledgement, Department_Responsible, Action_By, Approved_By, Rectification_Summary, Date_Resolved, Feedback_Status, " +
                "Feedback_Code, IsExcludedFromAnalysis, Remarks) VALUES " +
                "(@DateReceived, @ContributorName, @StudentId, @ContributorStatus, @CourseOrDepartment, @Phone, " +
                "@Email, @FeedbackNature, @Channel, @Title, @FeedbackSummary, @DateAcknowledged, @ResponsibleDepartment, @ActionBy, @ApprovedBy, @RectificationSummary, @DateResolved, @Progress, " +
                "@Category, @IsExcludedFromAnalysis, @Remarks)";

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            command.Parameters.AddWithValue("@DateReceived", feedback.DateReceived.ToShortDateString());
            command.Parameters.AddWithValue("@ContributorName", feedback.ContributorName);
            command.Parameters.AddWithValue("@StudentId", feedback.StudentId);
            command.Parameters.AddWithValue("@ContributorStatus", feedback.ContributorStatus);
            command.Parameters.AddWithValue("@CourseOrDepartment", feedback.Affiliation);
            command.Parameters.AddWithValue("@Phone", feedback.Phone);
            command.Parameters.AddWithValue("@Email", feedback.Email);
            command.Parameters.AddWithValue("@FeedbackNature", feedback.FeedbackNature);
            command.Parameters.AddWithValue("@Channel", feedback.Channel);
            command.Parameters.AddWithValue("@Title", feedback.Title);
            command.Parameters.AddWithValue("@FeedbackSummary", feedback.FeedbackSummary);
            command.Parameters.AddWithValue("@DateAcknowledged", feedback.DateAcknowledged.Value.ToShortDateString());
            command.Parameters.AddWithValue("@ResponsibleDepartment", feedback.ResponsibleDepartment);
            command.Parameters.AddWithValue("@ActionBy", feedback.ActionBy);
            command.Parameters.AddWithValue("@ApprovedBy", feedback.ApprovedBy);
            command.Parameters.AddWithValue("@RectificationSummary", feedback.RectificationSummary);
            command.Parameters.AddWithValue("@DateResolved", feedback.DateResolved.Value.ToShortDateString());
            command.Parameters.AddWithValue("@Progress", feedback.Progress);
            command.Parameters.AddWithValue("@Category", feedback.Category);
            command.Parameters.AddWithValue("@IsExcludedFromAnalysis", feedback.IsExcludedFromAnalysis);
            command.Parameters.AddWithValue("@Remarks", feedback.Remarks);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();

            return feedback;
        }

        public async Task<Feedback> UpdateFeedback(Feedback feedback)
        {
            string sql = "UPDATE Feedbacks SET " +
                "Date_Received = @DateReceived, " +
                "Contributor_Name = @ContributorName, " +
                "Student_ID = @StudentId, " +
                "Contributor_Status = @ContributorStatus, " +
                "Course_or_Department = @CourseOrDepartment, " +
                "Contact_No = @Phone, " +
                "Email = @Email, " +
                "Feedback_Nature = @FeedbackNature, " +
                "Channel = @Channel, " +
                "Title = @Title, " +
                "Feedback_Summary = @FeedbackSummary, " +
                "Date_Acknowledgement = @DateAcknowledged, " +
                "Department_Responsible = @ResponsibleDepartment, " +
                "Rectification_Summary = @RectificationSummary, " +
                "Action_By = @ActionBy, " +
                "Approved_By = @ApprovedBy, " +
                "Date_Resolved = @DateResolved, " +
                "Feedback_Status = @Progress, " +
                "Feedback_Code = @Category, " +
                "IsExcludedFromAnalysis = @IsExcludedFromAnalysis," +
                "Remarks = @Remarks " +
                "WHERE Id = " + feedback.Id.ToString();

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            command.Parameters.AddWithValue("@DateReceived", feedback.DateReceived.ToShortDateString());
            command.Parameters.AddWithValue("@ContributorName", feedback.ContributorName);
            command.Parameters.AddWithValue("@StudentId", feedback.StudentId);
            command.Parameters.AddWithValue("@ContributorStatus", feedback.ContributorStatus);
            command.Parameters.AddWithValue("@CourseOrDepartment", feedback.Affiliation);
            command.Parameters.AddWithValue("@Phone", feedback.Phone);
            command.Parameters.AddWithValue("@Email", feedback.Email);
            command.Parameters.AddWithValue("@FeedbackNature", feedback.FeedbackNature);
            command.Parameters.AddWithValue("@Channel", feedback.Channel);
            command.Parameters.AddWithValue("@Title", feedback.Title);
            command.Parameters.AddWithValue("@FeedbackSummary", feedback.FeedbackSummary);
            command.Parameters.AddWithValue("@DateAcknowledged", feedback.DateAcknowledged.Value.ToShortDateString());
            command.Parameters.AddWithValue("@ResponsibleDepartment", feedback.ResponsibleDepartment);
            command.Parameters.AddWithValue("@RectificationSummary", feedback.RectificationSummary);
            command.Parameters.AddWithValue("@ActionBy", feedback.ActionBy);
            command.Parameters.AddWithValue("@ApprovedBy", feedback.ApprovedBy);
            command.Parameters.AddWithValue("@DateResolved", feedback.DateResolved.Value.ToShortDateString());
            command.Parameters.AddWithValue("@Progress", feedback.Progress);
            command.Parameters.AddWithValue("@Category", feedback.Category);
            command.Parameters.AddWithValue("@IsExcludedFromAnalysis", feedback.IsExcludedFromAnalysis);
            command.Parameters.AddWithValue("@Remarks", feedback.Remarks);

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();

            return feedback;
        }

        public async void RemoveFeedback(Feedback feedback)
        {
            string sql = "UPDATE Feedbacks SET IsRemoved = @IsRemoved WHERE Id = " + feedback.Id.ToString();

            dbConnection.Open();
            command = new OleDbCommand(sql, dbConnection);

            command.Parameters.AddWithValue("@IsRemoved", "yes");

            await command.ExecuteNonQueryAsync();
            dbConnection.Close();
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            var departments = new List<Department>();

            departments.Add(new Department() { Name = "Academic", ShortName = "ACAD" });
            departments.Add(new Department() { Name = "Admissions", ShortName = "ADM" });
            departments.Add(new Department() { Name = "Facilities", ShortName = "FAC" });
            departments.Add(new Department() { Name = "Finance", ShortName = "FIN" });
            departments.Add(new Department() { Name = "Human Resource and Administration", ShortName = "HR" });
            departments.Add(new Department() { Name = "IT Support", ShortName = "IT" });
            departments.Add(new Department() { Name = "Library", ShortName = "LIB" });
            departments.Add(new Department() { Name = "Marketing", ShortName = "MKT" });
            departments.Add(new Department() { Name = "Navitas English", ShortName = "NEP" });
            departments.Add(new Department() { Name = "Quality and Compliance", ShortName = "QA" });
            departments.Add(new Department() { Name = "Student Administration", ShortName = "SA" });
            departments.Add(new Department() { Name = "Student Central", ShortName = "SC" });
            departments.Add(new Department() { Name = "Student Services", ShortName = "SSERV" });
            departments.Add(new Department() { Name = "Other", ShortName = "Other" });

            foreach (var department in departments)
            {
                department.Categories.Add("Proficiency");
                department.Categories.Add("Service rendered");
                department.Categories.Add("Response time");
                department.Categories.Add("Other");

                switch (department.ShortName)
                {
                    case "ACAD":
                    case "NEP":
                        department.Categories.Add("Course or unit contents");
                        department.Categories.Add("Lecturer proficiency");
                        department.Categories.Add("Lecturer quality of delivery");
                        break;
                    case "LIB":
                        department.Categories.Add("Facilities");
                        department.Categories.Add("Rules and regulations");
                        break;
                    case "SSERV":
                        department.Categories.Add("Enrichment classes");
                        department.Categories.Add("Graduation");
                        department.Categories.Add("Orientation");
                        break;
                    case "MKT":
                        department.Categories.Add("Agents' management");
                        department.Categories.Add("Marketing material");
                        break;
                    case "FIN":
                        department.Categories.Add("Financial matters");
                        department.Categories.Add("Waiting time");
                        break;
                    case "IT":
                        department.Categories.Add("Hardware problem");
                        department.Categories.Add("Support");
                        department.Categories.Add("System problem");
                        break;
                    case "FAC":
                        department.Categories.Add("Adhoc maintenance");
                        department.Categories.Add("Cleanliness");
                        department.Categories.Add("Environment");
                        department.Categories.Add("Facilities adequacy");
                        department.Categories.Add("Parking");
                        department.Categories.Add("Preventive maintenance");
                        department.Categories.Add("Scheduled maintenance");
                        break;
                }
            }

            return departments;
        }

        public IEnumerable<string> GetAffiliations()
        {
            var affifialtions = new List<string>();

            affifialtions.Add("Bachelor of Arts (Mass Communication)");
            affifialtions.Add("Bachelor of Commerce (Accounting and Finance)");
            affifialtions.Add("Bachelor of Commerce (Accounting)");
            affifialtions.Add("Bachelor of Commerce (Banking and Finance)");
            affifialtions.Add("Bachelor of Commerce (Finance and Marketing)");
            affifialtions.Add("Bachelor of Commerce (International Business)");
            affifialtions.Add("Bachelor of Commerce (Logistics and Supply Chain Management)");
            affifialtions.Add("Bachelor of Commerce (Management and Human Resource Management)");
            affifialtions.Add("Bachelor of Commerce (Management and Marketing)");
            affifialtions.Add("Bachelor of Commerce (Marketing and Advertising)");
            affifialtions.Add("Bachelor of Commerce (Marketing and Public Relations)");
            affifialtions.Add("Bachelor of Commerce (Marketing)");
            affifialtions.Add("Bachelor of Science (Nursing) Conversion Program For Registered Nurses (Top-Up)");
            affifialtions.Add("Certificate in General English (Elementary)");
            affifialtions.Add("Certificate in General English (Pre-intermediate)");
            affifialtions.Add("Diploma of Arts and Creative industries");
            affifialtions.Add("Diploma of Commerce");
            affifialtions.Add("Diploma of English For Academic Purposes");
            affifialtions.Add("Graduate Certificate in Clinical Nursing");
            affifialtions.Add("Clinical Nursing to Graduate Certificate in Wound, Ostomy and Continence Practice");
            affifialtions.Add("Graduate Certificate in Occupational Health and Safety Management");
            affifialtions.Add("Graduate Certificate in Project Management");
            affifialtions.Add("Graduate Diploma in Clinical Nursing");
            affifialtions.Add("Graduate Diploma in Wound, Ostomy and Continence Practice");
            affifialtions.Add("Graduate Diploma in International Business");
            affifialtions.Add("Graduate Diploma in Project Management");
            affifialtions.Add("Master of Business Administration (Global)");
            affifialtions.Add("Master of International Business");
            affifialtions.Add("Master of Occupational Health and Safety");
            affifialtions.Add("Master of Science (Clinical Leadership)");
            affifialtions.Add("Master of Science (Clinical Nursing)");
            affifialtions.Add("Master of Science (Health Practice)");
            affifialtions.Add("Master of Science (Project Management)");
            affifialtions.Add("Master of Supply Chain Management");
            affifialtions.Add("Postgraduate Diploma in Occupational Health and Safety");
            affifialtions.Add("Study Abroad Business");
            affifialtions.Add("Not for Degree");

            affifialtions.Add("Lecturer");
            affifialtions.Add("Prospective student");
            affifialtions.Add("Member of public");
            affifialtions.Add("Government agency or NGO");

            affifialtions.Add("Other");

            return affifialtions;
        }
    }
}
