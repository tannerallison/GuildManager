using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MinionsController : AuthorizedController
{
    public MinionsController(GMContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves all minions that are not currently employed.
    /// </summary>
    /// <returns></returns>
    // GET: api/Minions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Minion>>> GetMinions()
    {
        return await Context.Minions.Where(m => m.BossId == null).ToListAsync();
    }

    /// <summary>
    /// Retrieves a single, unemployed minion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/Minions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Minion>> GetMinion(int id)
    {
        var minion = await Context.Minions.FindAsync(id);

        if (minion == null)
        {
            return NotFound();
        }

        return minion;
    }


    // PATCH: api/Minions/5/hire
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Privilege.MinionHire)]
    [HttpPatch("{id}/hire")]
    public async Task<IActionResult> HireMinion(Guid id)
    {
        var player = Request.HttpContext.Items["Player"] as Player;
        var minion = Context.Minions.First(m => m.Id == id);
        if (minion.BossId.HasValue)
        {
            return Conflict();
        }

        minion.BossId = player.Id;
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
