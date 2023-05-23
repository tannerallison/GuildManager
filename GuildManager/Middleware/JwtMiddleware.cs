using Microsoft.Extensions.Options;
using GuildManager.Services;
using GuildManager.Utilities;

namespace GuildManager.Middleware;

/// <summary>
/// Example pulled from https://jasonwatmore.com/post/2021/12/14/net-6-jwt-authentication-tutorial-with-example-api#user-service-cs
/// </summary>

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            authenticationService.AttachUserToContext(context, token);

        await _next(context);
    }
}
