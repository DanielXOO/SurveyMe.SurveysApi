namespace Surveys.Api.Handlers;

public class AuthorizeHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _accessor;
    
    
    public AuthorizeHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string header = _accessor.HttpContext.Request.Headers.Authorization;

        if (string.IsNullOrEmpty(header))
        {
            //TODO: throw exception
        }
        
        request.Headers.Add("Authorization", header);
        
        return base.SendAsync(request, cancellationToken);
    }
}