using SurveyMe.Common.Pagination;

namespace Surveys.Api.Models.Request.Queries;

/// <summary>
/// Class for pagination query
/// </summary>
public sealed class GetPageRequest
{
    /// <summary>
    /// <value>Search term</value>
    /// </summary>
    public string? NameSearchTerm { get; set; } = "";
    
    /// <summary>
    /// <value>Name column sort order</value>
    /// </summary>
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    
    /// <summary>
    /// <value>Count of items per page</value>
    /// </summary>
    public int PageSize { get; set; } = 5;

    /// <summary>
    /// <value>Page number</value>
    /// </summary>
    public int Page { get; set; } = 1;
}