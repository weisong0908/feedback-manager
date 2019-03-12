using FeedbackManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FeedbackManager.WPF.Services
{
    public class WindowService : IWindowService
    {
        public void ShowChartsGeneratorWindow()
        {
            var window = new ChartsGeneratorWindow();

            window.ShowDialog();
        }

        public void ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(text, caption);
        }
    }
}
