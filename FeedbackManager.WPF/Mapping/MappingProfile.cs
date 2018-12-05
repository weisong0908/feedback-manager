using AutoMapper;
using FeedbackManager.WPF.Models;
using FeedbackManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManager.WPF.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Feedback, FeedbackViewModel>()
                .ForMember(fvm => fvm.IsExcludedFromAnalysis, opt => opt.MapFrom(f => (f.IsExcludedFromAnalysis == "yes")));

            CreateMap<FeedbackViewModel, Feedback>()
                .ForMember(f => f.IsExcludedFromAnalysis, opt => opt.MapFrom(fvm => (fvm.IsExcludedFromAnalysis) ? "yes" : string.Empty));
        }
    }
}
