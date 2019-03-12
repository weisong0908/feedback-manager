using FeedbackManager.WPF.Helpers;
using FeedbackManager.WPF.Models;
using FeedbackManager.WPF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.ViewModels
{
    public class ChartsGeneratorWindowViewModel : BaseViewModel
    {
        private DateTime _reportDate = DateTime.Today;
        public DateTime ReportDate
        {
            get { return _reportDate; }
            set { SetValue(ref _reportDate, value); }
        }

        private string _reportDestinationFolder;
        public string ReportDestinationFolder
        {
            get { return _reportDestinationFolder; }
            set { SetValue(ref _reportDestinationFolder, value); }
        }

        private string _progressMessage = "Ready";
        public string ProgressMessage
        {
            get { return _progressMessage; }
            set { SetValue(ref _progressMessage, value); }
        }

        private bool _isProgressBarRun;
        public bool IsProgressBarRun
        {
            get { return _isProgressBarRun; }
            set { SetValue(ref _isProgressBarRun, value); }
        }

        private readonly IEnumerable<Feedback> feedbacks;
        private readonly IFeedbackService feedbackService;
        private readonly IWindowService windowService;
        private ChartsGenerator chartsGenerator;

        public ChartsGeneratorWindowViewModel(IEnumerable<Feedback> feedbacks, IFeedbackService feedbackService, IWindowService windowService)
        {
            this.feedbacks = feedbacks.Where(f => f.IsExcludedFromAnalysis.ToLower() != "yes");
            this.feedbackService = feedbackService;
            this.windowService = windowService;
        }

        public void SetChartsDestinationFolder()
        {
            ReportDestinationFolder = windowService.SetChartsDestinationFolder();
        }

        public async void GenerateCharts()
        {
            if (_reportDate == DateTime.MinValue)
                return;

            if (!Directory.Exists(_reportDestinationFolder))
                return;

            chartsGenerator = new ChartsGenerator(feedbackService, feedbacks, _reportDate, _reportDestinationFolder);
            chartsGenerator.ChartCreated += OnChartCreated;
            IsProgressBarRun = true;

            await chartsGenerator.GenerateChartsAsync();

            IsProgressBarRun = false;
            ProgressMessage = "Ready";

            windowService.ShowMessageBox($"Charts have been saved at {_reportDestinationFolder}", "Charts generated");
        }

        private void OnChartCreated(object sender, string message)
        {
            ProgressMessage = message;
        }
    }
}
