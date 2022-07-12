namespace Surveys.Api.Models.Request.Options;

/// <summary>
/// Question option create model
/// </summary>
public sealed class QuestionOptionAddRequestModel
{
    /// <summary>
    /// <value>Option's text</value>
    /// </summary>
    public string Text { get; set; }
}