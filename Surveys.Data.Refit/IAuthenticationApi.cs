﻿using Refit;
using SurveyMe.AuthenticationApi.Models.Request;
using SurveyMe.AuthenticationApi.Models.Response;

namespace Surveys.Data.Refit;

public interface IAuthenticationApi
{
    [Post("/authentication-api/token")]
    Task<GetTokenResponseModel> GetTokenAsync([Body(BodySerializationMethod.UrlEncoded)]GetTokenRequestModel request);
}