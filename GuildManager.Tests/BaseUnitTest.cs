using System.Security.Claims;
using GuildManager.Controllers;
using GuildManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuildManager.Tests;

public class BaseUnitTest
{
    protected static Player SetUpAuthenticatedPlayer(AuthorizedController controller)
    {
        var player = new Player();
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "username"),
            new Claim(ClaimTypes.NameIdentifier, player.Id.ToString())
        }, "mock"));
        return player;
    }
    
}