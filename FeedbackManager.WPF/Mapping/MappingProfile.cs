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
                .ForMember(f => f.IsExcludedFromAnalysis, opt => opt.Ignore())
                .AfterMap((f, fvm) =>
                {
                    fvm.IsExcludedFromAnalysis = (f.IsExcludedFromAnalysis == "yes");
                });
            CreateMap<FeedbackViewModel, Feedback>()
                .ForMember(fvm => fvm.IsExcludedFromAnalysis, opt => opt.Ignore())
                .AfterMap((fvm, f) =>
                {
                    if (fvm.IsExcludedFromAnalysis)
                        f.IsExcludedFromAnalysis = "yes";
                    else
                        f.IsExcludedFromAnalysis = "";
                });
        }
    }
}
