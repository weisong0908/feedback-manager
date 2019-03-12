using FeedbackManager.WPF.Models;
using FeedbackManager.WPF.Services;
using FeedbackManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FeedbackManager.WPF.Views
{
    /// <summary>
    /// Interaction logic for ChartGeneratorWindow.xaml
    /// </summary>
    public partial class ChartsGeneratorWindow : Window
    {
        public ChartsGeneratorWindowViewModel ViewModel { get { return DataContext as ChartsGeneratorWindowViewModel; } set { DataContext = value; } }

        public ChartsGeneratorWindow(IEnumerable<Feedback> feedbacks)
        {
            IFeedbackService feedbackService= (Application.Current as App).FeedbackService;
            IWindowService windowService = (Application.Current as App).WindowService;

            InitializeComponent();

            ViewModel = new ChartsGeneratorWindowViewModel(feedbacks, feedbackService, windowService);
        }

        private void SetChartsDestination(object sender, RoutedEventArgs e)
        {
            ViewModel.SetChartsDestinationFolder();
        }

        private void GenerateCharts(object sender, RoutedEventArgs e)
        {
            ViewModel.GenerateCharts();
        }
    }
}
