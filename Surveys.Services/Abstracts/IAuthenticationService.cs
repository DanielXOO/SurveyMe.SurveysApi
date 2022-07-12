using SurveyMe.AuthenticationApi.Models.Request;

namespace Surveys.Services.Abstracts;

public interface IAuthenticationService
{
    Task<string> GetTokenAsync(GetTokenRequestModel request);
}