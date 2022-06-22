using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using SurveyMe.SurveyPersonApi.Models.Request.Options;
using Surveys.Data.Refit;
using Surveys.Models.SurveyOptions;
using Surveys.Services.Abstracts;

namespace Surveys.Services;

public class SurveyPersonService : ISurveyPersonService
{
    private readonly IMapper _mapper;

    private readonly ISurveyPersonApi _surveyPersonApi;

    private readonly IDistributedCache _cache;
    
    
    public SurveyPersonService(IMapper mapper, ISurveyPersonApi surveyPersonApi, IDistributedCache cache)
    {
        _mapper = mapper;
        _surveyPersonApi = surveyPersonApi;
        _cache = cache;
    }

    
    public async Task<SurveyOptions> GetSurveyPersonOptionsAsync(Guid id)
    {
        var serializedOptions = await _cache.GetStringAsync(id.ToString());

        if (!string.IsNullOrEmpty(serializedOptions))
        {
            var option = JsonSerializer.Deserialize<SurveyOptions>(serializedOptions);

            return option;
        }

        var optionsResponse = await _surveyPersonApi.GetSurveyPersonOptionsAsync(id);
        var options = _mapper.Map<SurveyOptions>(optionsResponse);
        
        serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(options.Id.ToString(), serializedOptions);

        return options;
    }

    public async Task EditSurveyPersonOptionsAsync(SurveyOptions options)
    {
        var optionsRequest = _mapper.Map<SurveyOptionsEditRequestModel>(options);
        
        await _surveyPersonApi.EditSurveyPersonOptionsAsync(optionsRequest, optionsRequest.Id);

        var serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(optionsRequest.Id.ToString(), serializedOptions);
    }

    public async Task DeleteSurveyPersonOptionsAsync(Guid id)
    {
        await _surveyPersonApi.DeleteSurveyPersonOptionsAsync(id);

        await _cache.RemoveAsync(id.ToString());
    }

    public async Task<Guid> AddOptionsAsync(SurveyOptions options)
    {
        var optionsRequest = _mapper.Map<SurveyOptionsCreateRequestModel>(options);
        var optionsId = await _surveyPersonApi.AddOptionsAsync(optionsRequest);
        
        var serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(options.Id.ToString(), serializedOptions);

        return optionsId;
    }
}