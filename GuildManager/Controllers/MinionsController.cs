using System.Data;
using GuildManager.DAL;
using Microsoft.AspNetCore.Mvc;
using GuildManager.Models;
using GuildManager.Utilities;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MinionsController : GenericController<Minion>
{
    public MinionsController(IUnitOfWork context) : base(context)
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
        return Ok(await UnitOfWork.GetRepository<Minion>().Get(m => m.BossId == null));
    }

    /// <summary>
    /// Retrieves a single, unemployed minion.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/minions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Minion>> GetMinion(Guid id)
    {
        var minion = await UnitOfWork.GetRepository<Minion>().GetById(id);

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
        await UnitOfWork.GetRepository<Minion>().Create(minion);
        await UnitOfWork.SaveAsync();

        return CreatedAtAction(nameof(GetMinion), new { id = minion.Id }, minion);
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
        var minion = await UnitOfWork.GetRepository<Minion>().GetById(id);
        if (minion == null)
        {
            return NotFound();
        }

        await UnitOfWork.GetRepository<Minion>().Delete(id);
        await UnitOfWork.SaveAsync();

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

        var minion = await UnitOfWork.GetRepository<Minion>().GetById(id);
        if (minion == null)
            return NotFound();
        if (minion.BossId.HasValue)
            return Conflict("Minion is already employed.");

        minion.BossId = player.Id;
        await UnitOfWork.GetRepository<Minion>().Update(minion);

        try
        {
            await UnitOfWork.SaveAsync();
        }
        catch (DataException)
        {
            ModelState.AddModelError("", "Unable to save changes. " +
                                         "Try again, and if the problem persists, " +
                                         "see your system administrator.");
        }

        return new OkResult();
    }
}
