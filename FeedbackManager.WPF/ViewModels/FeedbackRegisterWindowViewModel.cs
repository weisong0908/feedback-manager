using AutoMapper;
using FeedbackManager.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.ViewModels
{
    public class FeedbackRegisterWindowViewModel : BaseViewModel
    {
        private readonly IFeedbackService feedbackService;
        private readonly IMapper mapper;

        private ObservableCollection<FeedbackViewModel> _feedbacks;
        public ObservableCollection<FeedbackViewModel> Feedbacks
        {
            get { return _feedbacks; }
            set { SetValue(ref _feedbacks, value); }
        }

        private FeedbackViewModel _selectedFeedback;
        public FeedbackViewModel SelectedFeedback
        {
            get { return _selectedFeedback; }
            set { SetValue(ref _selectedFeedback, value); }
        }

        public FeedbackRegisterWindowViewModel(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
            mapper = Mapper.Instance;

        }

        public async void OnStartUp()
        {
            var feedbacks = mapper.Map<List<FeedbackViewModel>>((await feedbackService.GetAllFeedbacks()).OrderByDescending(f => f.DateReceived).ToList());
            Feedbacks = new ObservableCollection<FeedbackViewModel>(feedbacks);

            SelectedFeedback = _feedbacks.First();
        }
    }
}
