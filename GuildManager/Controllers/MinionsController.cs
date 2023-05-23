using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MinionsController : ControllerBase
{
    private readonly GMContext _context;

    public MinionsController(GMContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all minions that are not currently employed.
    /// </summary>
    /// <returns></returns>
    // GET: api/Minions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Minion>>> GetMinions()
    {
        return await _context.Minions.Where(m=>m.BossId == null).ToListAsync();
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
        var minion = await _context.Minions.FindAsync(id);

        if (minion == null)
        {
            return NotFound();
        }

        return minion;
    }


    // PATCH: api/Minions/5/hire
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPatch("{id}/hire")]
    public async Task<IActionResult> HireMinion(Guid id)
    {
        var player = Request.HttpContext.Items["Player"] as Player;
        var minion = _context.Minions.First(m => m.Id == id);
        if (minion.BossId.HasValue && minion.BossId.Value != player.Id)
        {
            return Conflict();
        }

        minion.BossId = player.Id;
        _context.Entry(minion).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MinionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool MinionExists(Guid id)
    {
        return (_context.Minions?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
