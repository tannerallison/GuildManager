using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuildManager.Controllers;

[Authorize]
[Route("api/my/minions")]
[ApiController]
public class MyMinionsController : AuthorizedController
{
    public MyMinionsController(GMContext context) : base(context)
    {
    }

    // GET: api/Minion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Minion>>> GetMinions()
    {
        if (!TryGetPlayer(out var player))
            return new UnauthorizedResult();

        return await Context.Minions.Where(m => m.BossId == player.Id).ToListAsync();
    }

    // GET: api/Minion/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Minion>> GetMinion(Guid id)
    {
        if (!TryGetPlayer(out var player))
            return new UnauthorizedResult();

        var minion = await Context.Minions.FirstAsync<Minion?>(c => c.BossId == player.Id && c.Id == id);

        if (minion == null)
            return NotFound();

        return minion;
    }

    // GET: api/Minion/5
    [HttpPatch("{id}/fire")]
    public async Task<ActionResult<Minion>> FireMinion(Guid id)
    {
        if (!TryGetPlayer(out var player))
            return new UnauthorizedResult();

        var minion = await Context.Minions.FirstAsync<Minion?>(c => c.BossId == player.Id && c.Id == id);

        if (minion == null)
            return NotFound();

        minion.BossId = null;
        Context.Entry(minion).State = EntityState.Modified;
        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MinionExists(id))
                return NotFound();

            throw;
        }

        return minion;
    }

    private bool MinionExists(Guid id)
    {
        return (Context.Minions?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
