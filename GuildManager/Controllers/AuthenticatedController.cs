using System.Diagnostics.CodeAnalysis;
using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuildManager.Controllers;

[Authenticate]
public abstract class AuthenticatedController : ControllerBase
{
    protected GMContext Context;

    protected AuthenticatedController(GMContext context)
    {
        Context = context;
    }

    protected bool TryGetPlayer([NotNullWhen(true)] out Player? player)
    {
        player = Request.HttpContext.Items["Player"] as Player;
        return player != null;
    }
}
