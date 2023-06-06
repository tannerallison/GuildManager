using GuildManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GuildManager.Utilities;

/// <summary>
/// Example pulled from https://jasonwatmore.com/post/2021/12/14/net-6-jwt-authentication-tutorial-with-example-api#user-service-cs
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string[] Privileges { get; set; }

    public AuthorizeAttribute(params string[] privileges)
    {
        Privileges = privileges;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (Player)context.HttpContext.Items["Player"];
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Authenticated Users Only" })
                { StatusCode = StatusCodes.Status401Unauthorized };
        }

        if (!Privileges.Any()) return;

        // check for authorized privileges
        if (!Privileges.Any(x => user.Roles.Any(y => y.Privileges.Any(z => z.Code == x))))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status403Forbidden };
        }
    }
}
