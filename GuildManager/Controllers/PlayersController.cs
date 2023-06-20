using GuildManager.DAL;
using Microsoft.AspNetCore.Mvc;
using GuildManager.Models;
using GuildManager.Utilities;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayersController : GenericController<Player>
{
    public PlayersController(IUnitOfWork context) : base(context)
    {
    }

    /// <summary>
    /// GET: api/Players
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        var players = await Repository.Get();
        return Ok(players);
    }

    /// <summary>
    /// GET: api/Players/5
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(Guid id)
    {
        var player = await Repository.GetById(id);

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
        if (string.IsNullOrEmpty(player.Username))
            return Problem("Username is required");
        if (string.IsNullOrEmpty(player.Password))
            return Problem("Password is required");

        var foundPlayer = await Repository.Get(filter: p => p.Username == player.Username);
        if (foundPlayer.Any())
        {
            return Problem("Username is already taken");
        }

        var dbPlayer = new Player
        {
            Username = player.Username,
            PasswordHash = new PasswordHash(player.Password).ToArray()
        };

        await Repository.Create(dbPlayer);
        await UnitOfWork.SaveAsync();

        return CreatedAtAction("GetPlayer", new { id = dbPlayer.Id }, dbPlayer);
    }
}
