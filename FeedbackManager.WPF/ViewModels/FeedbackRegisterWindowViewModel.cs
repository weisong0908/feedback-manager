using AutoMapper;
using FeedbackManager.WPF.Helpers;
using FeedbackManager.WPF.Models;
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
        private readonly IWindowService windowService;
        private readonly IMapper mapper;

        public IEnumerable<string> FeedbackChannels { get; set; }
        public IEnumerable<string> Progresses { get; set; }
        public IEnumerable<string> FeedbackNatures { get; set; }
        public IEnumerable<string> ResponsibleDepartments { get; set; }
        public IEnumerable<string> ContributorStatuses { get; set; }
        public IEnumerable<string> Affiliations { get; set; }

        private ObservableCollection<string> _categories;
        public ObservableCollection<string> Categories
        {
            get { return _categories; }
            set { SetValue(ref _categories, value); }
        }

        private ObservableCollection<FeedbackViewModel> _feedbacks;
        public ObservableCollection<FeedbackViewModel> Feedbacks
        {
            get { return _feedbacks; }
            set { SetValue(ref _feedbacks, value); }
        }
        public int UnclosedFeedbacksCount { get { return _feedbacks.Where(f => f.Progress != Progress.Closed).Count(); } }

        private FeedbackViewModel _selectedFeedback;
        public FeedbackViewModel SelectedFeedback
        {
            get { return _selectedFeedback; }
            set { SetValue(ref _selectedFeedback, value); }
        }

        public FeedbackRegisterWindowViewModel(IFeedbackService feedbackService, IWindowService windowService)
        {
            this.feedbackService = feedbackService;
            this.windowService = windowService;
            mapper = Mapper.Instance;

            FeedbackChannels = typeof(FeedbackChannel).GetFields().Select(f => f.GetValue(null).ToString());
            Progresses = typeof(Progress).GetFields().Select(f => f.GetValue(null).ToString());
            FeedbackNatures = typeof(FeedbackNature).GetFields().Select(f => f.GetValue(null).ToString());
            ContributorStatuses = typeof(ContributorStatus).GetFields().Select(f => f.GetValue(null).ToString());
            Categories = new ObservableCollection<string>();
            OnStartUp();
        }

        private async void OnStartUp()
        {
            var departments = feedbackService.GetAllDepartments().Select(d => d.Name).ToList();
            departments.Add("");
            ResponsibleDepartments = departments;
            Affiliations = feedbackService.GetAffiliations().Concat(ResponsibleDepartments);
            var feedbacks = mapper.Map<List<FeedbackViewModel>>((await feedbackService.GetAllFeedbacks()).OrderByDescending(f => f.DateReceived).ToList());
            Feedbacks = new ObservableCollection<FeedbackViewModel>(feedbacks);

            SelectedFeedback = _feedbacks.First();
        }

        public void UpdateCategories()
        {
            if (_selectedFeedback == null)
                return;

            if (string.IsNullOrEmpty(_selectedFeedback.ResponsibleDepartment))
                return;

            var categories = feedbackService.GetAllDepartments().Where(d => d.Name == _selectedFeedback.ResponsibleDepartment).SingleOrDefault().Categories;
            categories.Add("");
            Categories = new ObservableCollection<string>(categories);
        }

        public void AddNewFeedback()
        {
            SelectedFeedback = new FeedbackViewModel();
        }

        public async void SaveFeedback()
        {
            if (SelectedFeedback == null)
                return;

            var originalFeedbacks = Feedbacks;
            var feedbackForChange = mapper.Map<Feedback>(_selectedFeedback);
            object response;

            if (SelectedFeedback.Id == 0)
            {
                Feedbacks.Add(SelectedFeedback);
                Feedbacks = new ObservableCollection<FeedbackViewModel>(Feedbacks.OrderByDescending(fvm => fvm.DateReceived));

                response = await feedbackService.AddNewFeedback(feedbackForChange);
                if (response == null)
                {
                    Feedbacks = originalFeedbacks;
                }
            }
            else
            {
                response = await feedbackService.UpdateFeedback(feedbackForChange);
            }
        }

        public void RemoveFeedback()
        {
            var originalFeedbacks = Feedbacks;

            feedbackService.RemoveFeedback(mapper.Map<Feedback>(_selectedFeedback));
            Feedbacks.Remove(_selectedFeedback);
        }

        public void SendAcknowledgementEmail()
        {
            if (SelectedFeedback == null)
                return;

            EmailHelper.Send(_selectedFeedback.Email, _selectedFeedback.Title);
        }

        public void GoToChartsGeneratorWindow()
        {
            windowService.ShowChartsGeneratorWindow();
        }
    }
}
