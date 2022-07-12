using Surveys.Api.Models.Request.Options;
using Surveys.Models.Questions;

namespace Surveys.Api.Models.Request.Questions;

/// <summary>
/// Question creation model
/// </summary>
public sealed class QuestionAddRequestModel
{
    /// <summary>
    /// <value>Question title model</value>
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// <value>Question type</value>
    /// <example>
    /// Text, Radio, Checkbox, File, Rate, Scale
    /// </example>
    /// </summary>
    public QuestionType Type { get; set; }

    /// <summary>
    /// <value>Question options</value>
    /// </summary>
    public ICollection<QuestionOptionAddRequestModel>? Options { get; set; }
}