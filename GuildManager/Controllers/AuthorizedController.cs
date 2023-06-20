using System.Diagnostics.CodeAnalysis;
using GuildManager.DAL;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuildManager.Controllers;

[Authorize]
public abstract class AuthorizedController : ControllerBase
{
    protected IUnitOfWork UnitOfWork;

    protected AuthorizedController(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    protected async Task<Player?> GetPlayerAsync()
    {
        var contextPlayer = Request.HttpContext.Items["Player"] as Player;
        if (contextPlayer == null)
            return null;
        return await UnitOfWork.GetRepository<Player>().GetById(contextPlayer.Id);
    }

    protected bool TryGetPlayer([NotNullWhen(true)] out Player? player)
    {
        player = Request.HttpContext.Items["Player"] as Player;
        return player != null;
    }
}
