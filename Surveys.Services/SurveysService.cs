using AutoMapper;
using MassTransit;
using SurveyMe.Common.Exceptions;
using SurveyMe.Common.Pagination;
using SurveyMe.Common.Time;
using SurveyMe.QueueModels;
using Surveys.Data.Abstracts;
using Surveys.Models.SurveyOptions;
using Surveys.Models.Surveys;
using Surveys.Services.Abstracts;

namespace Surveys.Services;

public class SurveysService : ISurveysService
{
    private readonly ISurveysUnitOfWork _unitOfWork;

    private readonly ISurveyPersonService _surveyPersonService;
    
    private readonly ISystemClock _systemClock;
    
    private readonly IBus _bus;

    private readonly IMapper _mapper;

    
    public SurveysService(ISurveysUnitOfWork unitOfWork, ISystemClock systemClock, IBus bus, IMapper mapper,
        ISurveyPersonService surveyPersonService)
    {
        _unitOfWork = unitOfWork;
        _systemClock = systemClock;
        _bus = bus;
        _mapper = mapper;
        _surveyPersonService = surveyPersonService;
    }


    public async Task<PagedResult<Survey>> GetSurveysAsync(int currentPage, int pageSize,
        SortOrder order, string searchRequest)
    {
        var data = await _unitOfWork.Surveys
            .GetSurveysAsync(pageSize, currentPage, searchRequest, order);

        return data;
    }

    public async Task DeleteSurveyAsync(Survey survey)
    {
        await _unitOfWork.Surveys.DeleteAsync(survey);
        
        var surveyQueue = _mapper.Map<SurveyQueueModel>(survey);
        
        surveyQueue.EventType = EventType.Delete;

        await _bus.Publish(surveyQueue);
    }

    public async Task<Survey> GetSurveyByIdAsync(Guid id)
    {
        var survey = await _unitOfWork.Surveys.GetByIdAsync(id);

        if (survey == null)
        {
            throw new NotFoundException("Survey do not exist");
        }
            
        return survey;
    }

    public async Task AddSurveyAsync(Survey survey, Guid authorId, SurveyPersonOptions personOptions)
    {
        var surveyId = Guid.NewGuid();
        personOptions.SurveyId = surveyId;

        var optionsId = await _surveyPersonService.AddOptionsAsync(personOptions);
        
        survey.SurveyOptionId = optionsId;
        survey.Id = surveyId;
        
        survey.AuthorId = authorId;
        survey.LastChangeDate = _systemClock.UtcNow;
        await _unitOfWork.Surveys.CreateAsync(survey);

        var surveyQueue = _mapper.Map<SurveyQueueModel>(survey);

        surveyQueue.EventType = EventType.Create;
        
        await _bus.Publish(surveyQueue);
    }

    public async Task UpdateSurveyAsync(Survey survey)
    {
        survey.LastChangeDate = _systemClock.UtcNow;
        await _unitOfWork.Surveys.UpdateAsync(survey);
        
        var surveyQueue = _mapper.Map<SurveyQueueModel>(survey);
        
        surveyQueue.EventType = EventType.Update;
    }
}