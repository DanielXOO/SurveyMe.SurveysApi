using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using Surveys.Api.Models.Request.Questions;

namespace Surveys.Api.Models.Request.Surveys;

/// <summary>
/// Survey creation model
/// </summary>
public sealed class SurveyAddRequestModel
{
    /// <summary>
    /// <value>Name of survey</value>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// <value>Personality options</value>
    /// </summary>
    public SurveyOptionsCreateRequestModel Options { get; set; }
    
    /// <summary>
    /// <value>Questions collection</value>
    /// </summary>
    public ICollection<QuestionAddRequestModel> Questions { get; set; }
}