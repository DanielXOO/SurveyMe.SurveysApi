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

    private readonly ISurveyPersonApi _surveyPersonApi;

    private readonly IDistributedCache _cache;
    
    
    public SurveyPersonService(IMapper mapper, ISurveyPersonApi surveyPersonApi, IDistributedCache cache)
    {
        _mapper = mapper;
        _surveyPersonApi = surveyPersonApi;
        _cache = cache;
    }

    
    public async Task<SurveyPersonOptions> GetSurveyPersonOptionsAsync(Guid id)
    {
        var serializedOptions = await _cache.GetStringAsync(id.ToString());

        if (!string.IsNullOrEmpty(serializedOptions))
        {
            var option = JsonSerializer.Deserialize<SurveyPersonOptions>(serializedOptions);

            return option;
        }

        var optionsResponse = await _surveyPersonApi.GetSurveyPersonOptionsAsync(id);
        var options = _mapper.Map<SurveyPersonOptions>(optionsResponse);
        
        serializedOptions = JsonSerializer.Serialize(options);
        await _cache.SetStringAsync(options.Id.ToString(), serializedOptions);

        return options;
    }

    public async Task EditSurveyPersonOptionsAsync(SurveyPersonOptions personOptions)
    {
        var optionsRequest = _mapper.Map<SurveyOptionsEditRequestModel>(personOptions);
        
        await _surveyPersonApi.EditSurveyPersonOptionsAsync(optionsRequest, optionsRequest.SurveyOptionsId);

        var serializedOptions = JsonSerializer.Serialize(personOptions);
        await _cache.SetStringAsync(optionsRequest.SurveyOptionsId.ToString(), serializedOptions);
    }

    public async Task DeleteSurveyPersonOptionsAsync(Guid id)
    {
        await _surveyPersonApi.DeleteSurveyPersonOptionsAsync(id);

        await _cache.RemoveAsync(id.ToString());
    }

    public async Task<Guid> AddOptionsAsync(SurveyPersonOptions personOptions)
    {
        var optionsRequest = _mapper.Map<SurveyOptionsCreateRequestModel>(personOptions);
        var optionsId = await _surveyPersonApi.AddOptionsAsync(optionsRequest);
        
        var serializedOptions = JsonSerializer.Serialize(personOptions);
        await _cache.SetStringAsync(personOptions.Id.ToString(), serializedOptions);

        return optionsId;
    }
}