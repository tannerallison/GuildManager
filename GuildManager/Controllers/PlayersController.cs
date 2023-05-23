using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly GMContext _context;

    public PlayersController(GMContext context)
    {
        _context = context;
    }

    // GET: api/Players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        if (_context.Players == null)
        {
            return NotFound();
        }

        return await _context.Players.ToListAsync();
    }

    // GET: api/Players/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(Guid id)
    {
        if (_context.Players == null)
        {
            return NotFound();
        }

        var player = await _context.Players.FindAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        return player;
    }

    // PUT: api/Players/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlayer(Guid id, Player player)
    {
        if (id != player.Id)
        {
            return BadRequest();
        }

        _context.Entry(player).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlayerExists(id))
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

    // POST: api/Players
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer(string username)
    {
        if (_context.Players == null)
        {
            return Problem("Entity set 'GMContext.Players'  is null.");
        }

        if (username.IsNullOrEmpty())
        {
            return Problem("Username is required");
        }

        if (_context.Players.Any(p => p.Username == username))
        {
            return Problem("Username is already taken");
        }

        var player = new Player { Username = username };
        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
    }

    // DELETE: api/Players/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        if (_context.Players == null)
        {
            return NotFound();
        }

        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlayerExists(Guid id)
    {
        return (_context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
