using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Abstracts;

public interface ISurveyPersonService
{
    Task<SurveyPersonOptions> GetSurveyPersonOptionsAsync(Guid id);

    Task EditSurveyPersonOptionsAsync(SurveyPersonOptions personOptions);

    Task DeleteSurveyPersonOptionsAsync(Guid id);

    Task<Guid> AddOptionsAsync(SurveyPersonOptions personOptions);
}