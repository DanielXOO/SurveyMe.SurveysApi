using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Automapper.Profiles;

public class SurveyOptionsProfile : Profile
{
    public SurveyOptionsProfile()
    {
        CreateMap<SurveyOptions, SurveyOptionsCreateRequestModel>().ReverseMap();
        CreateMap<PersonalityOption, PersonalityOptionCreateRequestModel>();
        CreateMap<SurveyOptions, SurveyOptionsEditRequestModel>().ReverseMap();
        CreateMap<SurveyOptionsResponseModel, SurveyOptions>().ReverseMap();
    }
}