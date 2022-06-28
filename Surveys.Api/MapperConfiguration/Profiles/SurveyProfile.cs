using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using Surveys.Api.Models.Request.Surveys;
using Surveys.Api.Models.Response.Surveys;
using Surveys.Models.Surveys;

namespace Surveys.Api.MapperConfiguration.Profiles;

public sealed class SurveyProfile : Profile
{
    public SurveyProfile()
    {
        CreateMap<SurveyResponseModel, Survey>()
            .ReverseMap();
        
        CreateMap<SurveyAddRequestModel, Survey>()
            .ReverseMap();

        CreateMap<SurveyAddRequestModel, SurveyResponseModel>();

        CreateMap<SurveyOptionsCreateRequestModel, SurveyOptionsResponseModel>();
        
        CreateMap<SurveyEditRequestModel, Survey>()
            .ReverseMap();
    }
}