using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuildManager.Controllers;

[Route("api/my/minions")]
[ApiController]
public class MyMinionsController : AuthorizedController
{
    public MyMinionsController(GMContext context) : base(context)
    {
    }

    // GET: api/Minion
    /// <summary>
    /// Returns all minions belonging to the current player.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Minion>>> GetMinions()
    {
        if (!TryGetPlayer(out var player))
            return new UnauthorizedResult();

        return await Context.Minions.Where(m => m.BossId == player.Id).ToListAsync();
    }

    /// <summary>
    /// Retrieves a single minion belonging to the current player.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/my/minions/5
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

    /// <summary>
    /// Removes a minion from the current players employ and puts them back in the pool of available minions.
    /// Player cannot fire a minion that is currently on a Contract.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/my/minions/5
    [Authorize(Privilege.MinionFire)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> FireMinion(Guid id)
    {
        if (!TryGetPlayer(out var player))
            return new UnauthorizedResult();

        var minion = Context.Minions.Find(id);
        if (minion == null || minion.BossId != player.Id)
            return NotFound();

        if (minion.OnAJob())
            return new ForbidResult("Cannot fire a minion that is currently on a contract.");

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

        return new OkResult();
    }

    private bool MinionExists(Guid id)
    {
        return (Context.Minions?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
