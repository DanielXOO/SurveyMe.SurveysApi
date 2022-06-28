using SurveyMe.Common.Pagination;
using Surveys.Models.SurveyOptions;
using Surveys.Models.Surveys;

namespace Surveys.Services.Abstracts;

public interface ISurveysService
{
    Task<PagedResult<Survey>> GetSurveysAsync(int currentPage, int pageSize,
        SortOrder order, string searchRequest);

    Task DeleteSurveyAsync(Survey survey);

    Task<Survey> GetSurveyByIdAsync(Guid id);

    Task AddSurveyAsync(Survey survey, Guid authorId, SurveyOptions option);

    Task UpdateSurveyAsync(Survey survey);
}