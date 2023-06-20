using GuildManager.DAL;
using Microsoft.AspNetCore.Mvc;
using GuildManager.Models;
using GuildManager.Utilities;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : GenericController<Contract>
{
    public JobsController(IUnitOfWork context) : base(context)
    {
    }

    // GET: api/Contracts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contract>>> GetJobs()
    {
        return Ok(await Repository.Get(c => c.PatronId == null));
    }


    // GET: api/Contracts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Contract>> GetJob(Guid id)
    {
        var job = await Repository.GetById(id);

        if (job == null)
        {
            return NotFound();
        }

        return job;
    }

    [HttpPost("{id}/accept")]
    [Authorize(Privilege.JobAccept)]
    public async Task<ActionResult<Contract>> AcceptContract([FromRoute] Guid id)
    {
        var taskOut = Task.WhenAll(GetEntityAsync(id), GetPlayerAsync());

        if (!TryGetPlayer(out var player))
        {
            return Unauthorized();
        }

        var contract = await GetEntityAsync(id);
        if (contract == null)
        {
            return NotFound();
        }

        if (contract.PatronId != null)
        {
            return Forbid("This contract has already been accepted.");
        }

        contract.PatronId = player.Id;

        await Repository.Update(contract);

        await UnitOfWork.SaveAsync();

        return CreatedAtAction(nameof(GetJob), new { id = contract.Id }, contract);
    }

    // POST: api/Contracts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Contract>> PostJob(Contract contract)
    {
        await Repository.Create(contract)
            .ContinueWith(t => UnitOfWork.SaveAsync());

        return CreatedAtAction(nameof(GetJob), new { id = contract.Id }, contract);
    }

    // DELETE: api/Contracts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        var repository = Repository;
        var job = await repository.GetById(id);
        if (job == null)
        {
            return NotFound();
        }

        await repository.Delete(id);
        await UnitOfWork.SaveAsync();

        return NoContent();
    }
}
