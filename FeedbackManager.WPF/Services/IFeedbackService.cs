using FeedbackManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacks();
        Task<int> GetFeedbackID(Feedback feedback);
        Task<Feedback> AddNewFeedback(Feedback feedback);
        Task<Feedback> UpdateFeedback(Feedback feedback);
        void RemoveFeedback(Feedback feedback);
        IEnumerable<Department> GetAllDepartments();
        IEnumerable<string> GetAffiliations();
    }
}
