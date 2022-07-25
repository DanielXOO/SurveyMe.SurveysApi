using Refit;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;

namespace Surveys.Data.Refit;

public interface ISurveyPersonOptionsApi
{
    [Get("/survey-person-options-api/surveys/{surveyId}/surveyperson")]
    Task<SurveyOptionsResponseModel> GetSurveyPersonOptionsAsync(Guid surveyId);

    [Patch("/survey-person-options-api/surveys/{surveyId}/surveyperson/{id}")]
    Task EditSurveyPersonOptionsAsync([Body]SurveyOptionsEditRequestModel editRequestModel, Guid id, Guid surveyId);

    [Delete("/survey-person-options-api/surveys/{surveyId}/surveyperson/{id}")]
    Task DeleteSurveyPersonOptionsAsync(Guid id, Guid surveyId);

    [Post("/survey-person-options-api/surveys/{surveyId}/surveyperson")]
    Task<SurveyOptionsResponseModel> AddOptionsAsync([Body]SurveyOptionsCreateRequestModel createRequestModel, Guid surveyId);
}