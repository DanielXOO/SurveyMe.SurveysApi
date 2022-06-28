using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Automapper.Profiles;

public class SurveyOptionsProfile : Profile
{
    public SurveyOptionsProfile()
    {
        CreateMap<SurveyPersonOptions, SurveyOptionsCreateRequestModel>().ReverseMap();
        CreateMap<SurveyPersonOptions, SurveyOptionsEditRequestModel>().ReverseMap();
        CreateMap<SurveyOptionsResponseModel, SurveyPersonOptions>().ReverseMap();
    }
}