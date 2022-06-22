using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Abstracts;

public interface ISurveyPersonService
{
    Task<SurveyOptions> GetSurveyPersonOptionsAsync(Guid id);

    Task EditSurveyPersonOptionsAsync(SurveyOptions options);

    Task DeleteSurveyPersonOptionsAsync(Guid id);

    Task<Guid> AddOptionsAsync(SurveyOptions options);
}