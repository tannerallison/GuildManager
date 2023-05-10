using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GuildManager.Filters;

public class ApiKeyAuthAttribute : IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "ApiKey";

    private readonly GMContext _dbcontext;

    public ApiKeyAuthAttribute(GMContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var player = _dbcontext?.Players.FirstOrDefault(p => p.ApiKey == apiKey.ToString());

        if (player == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["Player"] = player;

        await next();
    }
}
