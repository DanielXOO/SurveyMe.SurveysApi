using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options;
using SurveyMe.SurveyPersonApi.Models.Response.Options;
using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Automapper.Profiles;

public class SurveyOptionsProfile : Profile
{
    public SurveyOptionsProfile()
    {
        CreateMap<SurveyOptions, SurveyOptionsCreateRequestModel>().ReverseMap();
        CreateMap<SurveyOptions, SurveyOptionsEditRequestModel>().ReverseMap();
        CreateMap<SurveyOptionsResponseModel, SurveyOptions>().ReverseMap();
    }
}