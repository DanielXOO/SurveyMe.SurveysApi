using SurveyMe.SurveyPersonApi.Models.Response.Options;
using Surveys.Api.Models.Response.Questions;

namespace Surveys.Api.Models.Response.Surveys;

public sealed class SurveyResponseModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public SurveyOptionsResponseModel Options { get; set; }
    
    public ICollection<QuestionResponseModel> Questions { get; set; }
}