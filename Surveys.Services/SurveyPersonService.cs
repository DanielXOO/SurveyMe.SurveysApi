using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using Surveys.Data.Refit;
using Surveys.Models.SurveyOptions;
using Surveys.Services.Abstracts;

namespace Surveys.Services;

public class SurveyPersonService : ISurveyPersonService
{
    private readonly IMapper _mapper;

    private readonly ISurveyPersonOptionsApi _surveyPersonOptionsApi;

    private readonly IDistributedCache _cache;
    
    
    public SurveyPersonService(IMapper mapper, ISurveyPersonOptionsApi surveyPersonOptionsApi, IDistributedCache cache)
    {
        _mapper = mapper;
        _surveyPersonOptionsApi = surveyPersonOptionsApi;
        _cache = cache;
    }

    
    public async Task<SurveyOptions> GetSurveyOptionsByIdAsync(Guid surveyId)
    {
        var serializedOptions = await _cache.GetStringAsync(surveyId.ToString());

        if (!string.IsNullOrEmpty(serializedOptions))
        {
            var option = JsonSerializer.Deserialize<SurveyOptions>(serializedOptions);

            return option;
        }

        var optionsResponse = await _surveyPersonOptionsApi.GetSurveyPersonOptionsAsync(surveyId);
        var options = _mapper.Map<SurveyOptions>(optionsResponse);
        
        serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(options.SurveyId.ToString(), serializedOptions);

        return options;
    }

    public async Task EditSurveyOptionsAsync(SurveyOptions options, Guid id, Guid surveyId)
    {
        var optionsRequest = _mapper.Map<SurveyOptionsEditRequestModel>(options);
        
        await _surveyPersonOptionsApi.EditSurveyPersonOptionsAsync(optionsRequest, optionsRequest.SurveyOptionsId,
            optionsRequest.SurveyId);

        var serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(optionsRequest.SurveyOptionsId.ToString(), serializedOptions);
    }

    public async Task DeleteSurveyOptionsAsync(Guid id, Guid surveyId)
    {
        await _surveyPersonOptionsApi.DeleteSurveyPersonOptionsAsync(id, surveyId);

        await _cache.RemoveAsync(id.ToString());
    }

    public async Task<SurveyOptions> AddSurveyOptionsAsync(SurveyOptions options, Guid surveyId)
    {
        var optionsRequest = _mapper.Map<SurveyOptionsCreateRequestModel>(options);
        var optionsResponse = await _surveyPersonOptionsApi.AddOptionsAsync(optionsRequest, surveyId);

        _mapper.Map(optionsResponse, options);
        
        var serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(options.SurveyId.ToString(), serializedOptions);

        return options;
    }
}