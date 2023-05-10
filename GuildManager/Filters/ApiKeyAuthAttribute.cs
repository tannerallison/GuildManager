using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GuildManager.Filters;

public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "ApiKey";

    private readonly IServiceProvider _serviceProvider;

    public ApiKeyAuthAttribute(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var gmContext = _serviceProvider.GetService<GMContext>();
        var player = gmContext?.Players.FirstOrDefault(p => p.ApiKey == apiKey);

        if (player == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["Player"] = player;

        await next();
    }
}
