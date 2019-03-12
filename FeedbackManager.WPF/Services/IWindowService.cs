using FeedbackManager.WPF.Models;
using System.Collections.Generic;

namespace FeedbackManager.WPF.Services
{
    public interface IWindowService
    {
        void ShowChartsGeneratorWindow(IEnumerable<Feedback> feedbacks);
        void ShowMessageBox(string text, string caption);
        string SetChartsDestinationFolder();
    }
}