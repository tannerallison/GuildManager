using GuildManager.Data;
using GuildManager.Filters;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuildManager.Controllers.My;

[ServiceFilter(typeof(ApiKeyAuth))]
[Route("api/my/[controller]")]
[ApiController]
public class MinionsController : ControllerBase
{
    private readonly GMContext _context;

    public MinionsController(GMContext context)
    {
        _context = context;
    }

    // GET: api/Minion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Minion>>> GetMinions()
    {
        var player = Request.HttpContext.Items["Player"] as Player;
        if (player == null)
            return new UnauthorizedResult();

        return await _context.Minions.Where(m => m.BossId == player.Id).ToListAsync();
    }

    // GET: api/Minion/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Minion>> GetMinion(int id)
    {
        var playerId = (Request.HttpContext.Items["Player"] as Player)?.Id ?? -1;
        var minion = await _context.Minions.FirstAsync<Minion?>(c => c.BossId == playerId && c.Id == id);

        if (minion == null)
        {
            return NotFound();
        }

        return minion;
    }

    // GET: api/Minion/5
    [HttpPatch("{id}/fire")]
    public async Task<ActionResult<Minion>> FireMinion(int id)
    {
        var playerId = (Request.HttpContext.Items["Player"] as Player)?.Id ?? -1;
        var minion = await _context.Minions.FirstAsync<Minion?>(c => c.BossId == playerId && c.Id == id);

        if (minion == null)
        {
            return NotFound();
        }

        minion.BossId = null;
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

        return minion;
    }

    private bool MinionExists(int id)
    {
        return (_context.Minions?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
