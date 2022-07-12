using Surveys.Api.Models.Request.Options;
using Surveys.Models.Questions;

namespace Surveys.Api.Models.Request.Questions;

/// <summary>
/// Question edit model
/// </summary>
public sealed class QuestionEditRequestModel
{
    /// <summary>
    /// <value>Question title</value>
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
    public ICollection<QuestionOptionEditRequestModel>? Options { get; set; }
}