using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayersController : AuthorizedController
{
    public PlayersController(GMContext context) : base(context)
    {
    }

    /// <summary>
    /// GET: api/Players
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        return await Context.Players.ToListAsync();
    }

    /// <summary>
    /// GET: api/Players/5
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// POST: api/Players
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer(PlayerRegisterDTO player)
    {
        if (player.Username.IsNullOrEmpty())
            return Problem("Username is required");
        if (player.Password.IsNullOrEmpty())
            return Problem("Password is required");

        if (Context.Players.Any(p => p.Username == player.Username))
        {
            return Problem("Username is already taken");
        }

        var dbPlayer = new Player
        {
            Username = player.Username,
            PasswordHash = new PasswordHash(player.Password).ToArray()
        };

        Context.Players.Add(dbPlayer);
        await Context.SaveChangesAsync();

        return CreatedAtAction("GetPlayer", new { id = dbPlayer.Id }, dbPlayer);
    }

    private bool PlayerExists(Guid id)
    {
        return (Context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
