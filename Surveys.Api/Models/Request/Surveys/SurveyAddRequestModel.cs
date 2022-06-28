using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using Surveys.Api.Models.Request.Questions;

namespace Surveys.Api.Models.Request.Surveys;

public sealed class SurveyAddRequestModel
{
    public string Name { get; set; }

    public SurveyOptionsCreateRequestModel Options { get; set; }
    
    public ICollection<QuestionAddRequestModel> Questions { get; set; }
}