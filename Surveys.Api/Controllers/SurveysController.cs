using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyMe.Common.Exceptions;
using SurveyMe.Error.Models.Response;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;
using Surveys.Api.Models.Request.Queries;
using Surveys.Api.Models.Request.Surveys;
using Surveys.Api.Models.Response.Pages;
using Surveys.Api.Models.Response.Surveys;
using Surveys.Models.SurveyOptions;
using Surveys.Models.Surveys;
using Surveys.Services.Abstracts;

namespace Surveys.Api.Controllers;

/// <summary>
/// Controller for interaction with surveys
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SurveysController : Controller
{
    private readonly ISurveysService _surveyService;
    
    private readonly IMapper _mapper;

    private readonly ISurveyPersonService _surveyPersonService;

    /// <summary>
    /// Controller's class constructor
    /// </summary>
    /// <param name="surveyService">service for surveys</param>
    /// <param name="mapper">mapping class</param>
    /// <param name="surveyPersonService">service for interaction with survey personality options</param>
    public SurveysController(ISurveysService surveyService, IMapper mapper, ISurveyPersonService surveyPersonService)
    {
        _surveyService = surveyService;
        _mapper = mapper;
        _surveyPersonService = surveyPersonService;
    }

    /// <summary>
    /// Endpoint for getting page with surveys
    /// </summary>
    /// <param name="request">Query for getting page as size, page number and etc</param>
    /// <returns>Paginated surveys list</returns>
    /// <exception cref="BadRequestException">Throws if query has null value</exception>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PageResponseModel<SurveyResponseModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseErrorResponse))]
    [HttpGet]
    public async Task<IActionResult> GetSurveysPage([FromQuery] GetPageRequest request)
    {
        if (request == null)
        {
            throw new BadRequestException("Request is empty");
        }
        
        var surveys = await _surveyService
            .GetSurveysAsync(request.Page, request.PageSize, request.SortOrder, request.NameSearchTerm);

        var options = new List<SurveyOptions>();

        foreach (var survey in surveys.Items)
        {
            var option = await _surveyPersonService.GetSurveyOptionsByIdAsync(survey.Id);
            options.Add(option);
        }

        var surveyResponse = surveys.Items.Join(options,
            survey => survey.Id,
            option => option.SurveyId,
            (survey, option) =>
            {
                var surveyResponse = _mapper.Map<SurveyResponseModel>(survey);
                surveyResponse.Options = _mapper.Map<SurveyOptionsResponseModel>(option);

                return surveyResponse;
            }).ToList();
        
        var pageResponse = new PageResponseModel<SurveyResponseModel>
        {
            NameSearchTerm = request.NameSearchTerm,
            SortOrder = request.SortOrder,
            Page = new PagedResultResponseModel<SurveyResponseModel>
            {
                CurrentPage = surveys.CurrentPage,
                PageSize = surveys.PageSize,
                TotalItems = surveys.TotalItems,
                Items = surveyResponse
            }
        };

        if (surveys.TotalPages < surveys.CurrentPage && surveys.TotalPages > 0)
        {
            return RedirectToAction(nameof(GetSurveysPage), request);
        }

        return Ok(pageResponse);
    }
    
    /// <summary>
    /// Endpoint for creation surveys
    /// </summary>
    /// <param name="surveyRequest">Survey model</param>
    /// <returns>Created survey</returns>
    /// <exception cref="BadRequestException">If surveys field invalid</exception>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseErrorResponse))]
    [HttpPost]
    public async Task<IActionResult> AddSurvey([FromBody] SurveyAddRequestModel surveyRequest)
    {
        var authorId = Guid.Parse(HttpContext.User.GetSubjectId());

        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                error => error.Key,
                error => error.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            
            throw new BadRequestException("Invalid data", errors);
        }

        var options = _mapper.Map<SurveyOptions>(surveyRequest.Options);
        var survey = _mapper.Map<Survey>(surveyRequest);

        var surveyWithOptions = await _surveyService.AddSurveyAsync(survey, authorId, options);

        var surveyResponse = _mapper.Map<SurveyResponseModel>(surveyWithOptions);
        
        return CreatedAtAction(Url.Action(nameof(GetSurvey)), surveyResponse);
    }
    
    /// <summary>
    /// Endpoint for getting survey
    /// </summary>
    /// <param name="id">Survey id</param>
    /// <returns>Survey model</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SurveyResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseErrorResponse))]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSurvey(Guid id)
    {
        var survey = await _surveyService.GetSurveyByIdAsync(id);
        var surveyResponseModel = _mapper.Map<SurveyResponseModel>(survey);
        
        var options = await _surveyPersonService.GetSurveyOptionsByIdAsync(survey.Id);
        var optionsResponse = _mapper.Map<SurveyOptionsResponseModel>(options);
        surveyResponseModel.Options = optionsResponse;
        
        return Ok(surveyResponseModel);
    }
    
    /// <summary>
    /// Endpoint for delete survey
    /// </summary>
    /// <param name="id">Survey id</param>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseErrorResponse))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSurvey(Guid id)
    {
        var survey = await _surveyService.GetSurveyByIdAsync(id);

        await _surveyPersonService.DeleteSurveyOptionsAsync(survey.SurveyOptionId, survey.Id);
        await _surveyService.DeleteSurveyAsync(survey);
        
        return NoContent();
    }

    /// <summary>
    /// Endpoint for edit survey
    /// </summary>
    /// <param name="surveyRequest">Survey edit model</param>
    /// <param name="id">Survey id</param>
    /// <exception cref="BadRequestException">Throws if ids do not match</exception>
    /// <exception cref="ForbidException">If user try edit someone else's survey</exception>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseErrorResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseErrorResponse))]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> EditSurvey([FromBody] SurveyEditRequestModel surveyRequest, Guid id)
    {
        if (surveyRequest.Id != id)
        {
            throw new BadRequestException("Route id and request id do not match");
        }
        
        var survey = await _surveyService.GetSurveyByIdAsync(id);

        var userId = Guid.Parse(HttpContext.User.GetSubjectId());

        var isAdmin = HttpContext.User.IsInRole("ADMIN");
        if (userId != survey.AuthorId && !isAdmin)
        {
            throw new ForbidException("Action denied");
        }

        _mapper.Map(surveyRequest, survey);

        var options = _mapper.Map<SurveyOptions>(surveyRequest);
        
        await _surveyService.UpdateSurveyAsync(survey);
        await _surveyPersonService.EditSurveyOptionsAsync(options, options.SurveyOptionsId, options.SurveyId);

        return NoContent();
    }
}