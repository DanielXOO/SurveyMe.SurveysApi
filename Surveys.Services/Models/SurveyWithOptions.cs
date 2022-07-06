using Surveys.Models.Questions;
using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Models;

public class SurveyWithOptions
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime? LastChangeDate { get; set; }

    public Guid AuthorId { get; set; }

    public ICollection<Question> Questions { get; set; }
    
    public SurveyOptions Options { get; set; }
}