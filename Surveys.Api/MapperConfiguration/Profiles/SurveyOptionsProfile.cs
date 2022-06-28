using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using Surveys.Models.SurveyOptions;

namespace Surveys.Api.MapperConfiguration.Profiles;

public class SurveyOptionsProfile : Profile
{
    public SurveyOptionsProfile()
    {
        CreateMap<SurveyOptionsCreateRequestModel, SurveyOptions>();
        CreateMap<PersonalityOptionCreateRequestModel, PersonalityOption>();
        CreateMap<SurveyOptionsResponseModel, SurveyOptions>().ReverseMap();
        CreateMap<PersonalityOptionResponseModel, PersonalityOption>().ReverseMap();
    }
}