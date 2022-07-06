using Surveys.Models.SurveyOptions;

namespace Surveys.Services.Abstracts;

public interface ISurveyPersonService
{
    Task<SurveyOptions> GetSurveyOptionsByIdAsync(Guid surveyId);

    Task EditSurveyOptionsAsync(SurveyOptions options, Guid id, Guid surveyId);

    Task DeleteSurveyOptionsAsync(Guid id, Guid surveyId);

    Task<SurveyOptions> AddSurveyOptionsAsync(SurveyOptions options, Guid surveyId);
}