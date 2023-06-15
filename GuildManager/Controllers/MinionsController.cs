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
    // GET: api/minions
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
    // GET: api/minions/5
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

    /// <summary>
    /// Creates a new minion.
    /// </summary>
    /// <param name="minion"></param>
    /// <returns></returns>
    // POST: api/minions
    [Authorize(Privilege.MinionCreate)]
    [HttpPost]
    public async Task<ActionResult<Minion>> CreateMinion(Minion minion)
    {
        Context.Minions.Add(minion);
        await Context.SaveChangesAsync();

        return CreatedAtAction("GetMinion", new { id = minion.Id }, minion);
    }

    /// <summary>
    /// Deletes a minion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // DELETE: api/minions/:id
    [Authorize(Privilege.MinionDelete)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMinion(Guid id)
    {
        var minion = await Context.Minions.FindAsync(id);
        if (minion == null)
        {
            return NotFound();
        }

        Context.Minions.Remove(minion);
        await Context.SaveChangesAsync();

        return NoContent();
    }

    // PATCH: api/Minions/5/hire
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Privilege.MinionHire)]
    [HttpPatch("{id}/hire")]
    public async Task<IActionResult> HireMinion(Guid id)
    {
        if (!TryGetPlayer(out var player))
            return Unauthorized();

        var minion = await Context.Minions.FindAsync(id);
        if (minion == null)
            return NotFound();
        if (minion.BossId.HasValue)
            return Conflict("Minion is already employed.");

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
