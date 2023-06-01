using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayersController : AuthenticatedController
{
    public PlayersController(GMContext context) : base(context)
    {
    }

    // GET: api/Players
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        if (Context.Players == null)
        {
            return NotFound();
        }

        return await Context.Players.ToListAsync();
    }

    // GET: api/Players/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(Guid id)
    {
        if (Context.Players == null)
        {
            return NotFound();
        }

        var player = await Context.Players.FindAsync(id);

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

        Context.Entry(player).State = EntityState.Modified;

        try
        {
            await Context.SaveChangesAsync();
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
        if (Context.Players == null)
        {
            return Problem("Entity set 'GMContext.Players'  is null.");
        }

        if (username.IsNullOrEmpty())
        {
            return Problem("Username is required");
        }

        if (Context.Players.Any(p => p.Username == username))
        {
            return Problem("Username is already taken");
        }

        var player = new Player { Username = username };
        Context.Players.Add(player);
        await Context.SaveChangesAsync();

        return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
    }

    // DELETE: api/Players/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        if (Context.Players == null)
        {
            return NotFound();
        }

        var player = await Context.Players.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        Context.Players.Remove(player);
        await Context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlayerExists(Guid id)
    {
        return (Context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
