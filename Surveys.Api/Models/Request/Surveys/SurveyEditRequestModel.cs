using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using Surveys.Api.Models.Request.Questions;

namespace Surveys.Api.Models.Request.Surveys;

/// <summary>
/// Model for edit survey
/// </summary>
public sealed class SurveyEditRequestModel
{
    /// <summary>
    /// <value>Survey id</value>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// <value>Survey title</value>
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// <value>Survey personality options</value>
    /// </summary>
    public SurveyOptionsEditRequestModel Options { get; set; }
    
    /// <summary>
    /// <value>Survey questions</value>
    /// </summary>
    public ICollection<QuestionEditRequestModel> Questions { get; set; }
}