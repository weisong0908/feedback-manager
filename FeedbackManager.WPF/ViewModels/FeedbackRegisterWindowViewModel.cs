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

        public ObservableCollection<string> Categories { get; set; }

        public ObservableCollection<FeedbackViewModel> Feedbacks { get; set; }

        public int UnclosedFeedbacksCount { get { return Feedbacks.Where(f=>f.Progress!=Progress.Closed).Count(); } }

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

            FeedbackChannels = FeedbackChannel.FeedbackChannels;
            Progresses = Progress.Progresses;
            FeedbackNatures = FeedbackNature.FeedbackNatures;
            ContributorStatuses = ContributorStatus.ContributorStatuses;
            Categories = new ObservableCollection<string>();
            OnStartUp();
        }

        private async void OnStartUp()
        {
            var departments = feedbackService.GetAllDepartments().Select(d => d.Name).ToList();
            departments.Insert(0, "");
            ResponsibleDepartments = departments;

            var affiliations = feedbackService.GetAffiliations().ToList();
            affiliations.AddRange(departments);
            affiliations.Remove("");
            affiliations.Insert(0, "");
            Affiliations = affiliations;

            var feedbacks = mapper.Map<List<FeedbackViewModel>>((await feedbackService.GetAllFeedbacks()).OrderByDescending(f => f.DateReceived).ToList());
            Feedbacks = new ObservableCollection<FeedbackViewModel>(feedbacks);

            SelectedFeedback = Feedbacks.First();
        }

        public void UpdateCategories()
        {
            if (_selectedFeedback == null)
                return;

            if (string.IsNullOrEmpty(_selectedFeedback.ResponsibleDepartment))
                return;

            var categories = feedbackService.GetAllDepartments().Where(d => d.Name == _selectedFeedback.ResponsibleDepartment).SingleOrDefault().Categories;
            categories.Insert(0, "");

            Categories.Clear();
            foreach (var category in categories)
                Categories.Add(category);
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

            if (feedbackForChange.Id == 0)
            {
                Feedbacks.Add(SelectedFeedback);
                Feedbacks = new ObservableCollection<FeedbackViewModel>(Feedbacks.OrderByDescending(fvm => fvm.DateReceived));

                response = await feedbackService.AddNewFeedback(feedbackForChange);
                if (response == null)
                {
                    Feedbacks = originalFeedbacks;
                }
                else
                {
                    SelectedFeedback.Id = await feedbackService.GetFeedbackID(response as Feedback);
                }
            }
            else
            {
                response = await feedbackService.UpdateFeedback(feedbackForChange);
            }

            OnPropertyChanged(nameof(UnclosedFeedbacksCount));
        }

        public void RemoveFeedback()
        {
            var originalFeedbacks = Feedbacks;

            feedbackService.RemoveFeedback(mapper.Map<Feedback>(_selectedFeedback));
            Feedbacks.Remove(_selectedFeedback);

            OnPropertyChanged(nameof(UnclosedFeedbacksCount));
        }

        public void SendAcknowledgementEmail()
        {
            if (SelectedFeedback == null)
                return;

            EmailHelper.Send(_selectedFeedback.Email, _selectedFeedback.Title);
        }

        public void GoToChartsGeneratorWindow()
        {
            windowService.ShowChartsGeneratorWindow(mapper.Map<IEnumerable<Feedback>>(Feedbacks));
        }
    }
}
