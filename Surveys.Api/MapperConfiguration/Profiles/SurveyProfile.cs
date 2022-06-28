using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using Surveys.Api.Models.Request.Options;
using Surveys.Api.Models.Request.Questions;
using Surveys.Api.Models.Request.Surveys;
using Surveys.Api.Models.Response.Questions;
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
        CreateMap<QuestionAddRequestModel, QuestionResponseModel>();
        CreateMap<QuestionOptionAddRequestModel, QuestionOptionResponseModel>();

        CreateMap<SurveyOptionsCreateRequestModel, SurveyOptionsResponseModel>();
        CreateMap<PersonalityOptionCreateRequestModel, PersonalityOptionResponseModel>();
        
        CreateMap<SurveyEditRequestModel, Survey>()
            .ReverseMap();
    }
}