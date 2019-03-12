using FeedbackManager.WPF.Models;
using FeedbackManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FeedbackManager.WPF.Services
{
    public class WindowService : IWindowService
    {
        public void ShowChartsGeneratorWindow(IEnumerable<Feedback> feedbacks)
        {
            var window = new ChartsGeneratorWindow(feedbacks);

            window.ShowDialog();
        }

        public void ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }

        public string SetChartsDestinationFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Export charts to folder"
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return string.Empty;
        }
    }
}
