using System.Diagnostics.CodeAnalysis;
using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuildManager.Controllers;

[Authorize]
public abstract class AuthorizedController : ControllerBase
{
    protected GMContext Context;

    protected AuthorizedController(GMContext context)
    {
        Context = context;
    }

    protected bool TryGetPlayer([NotNullWhen(true)] out Player? player)
    {
        player = Request.HttpContext.Items["Player"] as Player;
        return player != null;
    }
}
