using Refit;
using SurveyMe.SurveyPersonApi.Models.Request.Options;
using SurveyMe.SurveyPersonApi.Models.Response.Options;

namespace Surveys.Data.Refit;

public interface ISurveyPersonApi
{
    [Get("/api/surveyperson/{id}")]
    Task<SurveyOptionsResponseModel> GetSurveyPersonOptionsAsync(Guid id);

    [Patch("/api/surveyperson/{id}")]
    Task EditSurveyPersonOptionsAsync([Body]SurveyOptionsEditRequestModel editRequestModel, Guid id);

    [Delete("/api/surveyperson/{id}")]
    Task DeleteSurveyPersonOptionsAsync(Guid id);

    [Post("/api/surveyperson/")]
    Task<Guid> AddOptionsAsync([Body]SurveyOptionsCreateRequestModel createRequestModel);
}