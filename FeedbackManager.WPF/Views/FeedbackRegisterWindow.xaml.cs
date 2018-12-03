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
    /// Interaction logic for FeedbackRegisterWindow.xaml
    /// </summary>
    public partial class FeedbackRegisterWindow : Window
    {
        public FeedbackRegisterWindowViewModel ViewModel { get { return DataContext as FeedbackRegisterWindowViewModel; } set { DataContext = value; } }

        public FeedbackRegisterWindow()
        {
            InitializeComponent();

            var feedbackService = (Application.Current as App).FeedbackService;
            ViewModel = new FeedbackRegisterWindowViewModel(feedbackService);
            ViewModel.OnStartUp();
        }
    }
}
