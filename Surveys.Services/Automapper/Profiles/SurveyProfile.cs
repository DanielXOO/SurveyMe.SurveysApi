using AutoMapper;
using Surveys.Models.Surveys;
using Surveys.Services.Models;

namespace Surveys.Services.Automapper.Profiles;

public class SurveyProfile : Profile
{
    public SurveyProfile()
    {
        CreateMap<Survey, SurveyWithOptions>();
    }
}