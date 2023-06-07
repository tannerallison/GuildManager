using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : AuthorizedController
{
    public JobsController(GMContext context) : base(context)
    {
    }

    // GET: api/Contracts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contract>>> GetJobs()
    {
        return await Context.Contracts.Where(j => j.PatronId == null).ToListAsync();
    }

    // GET: api/Contracts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Contract>> GetJob(Guid id)
    {
        var job = await Context.Contracts.FindAsync(id);

        if (job == null)
        {
            return NotFound();
        }

        return job;
    }

    // PUT: api/Contracts/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJob(Guid id, Contract contract)
    {
        if (id != contract.Id)
        {
            return BadRequest();
        }

        Context.Entry(contract).State = EntityState.Modified;

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JobExists(id))
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

    // POST: api/Contracts
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Contract>> PostJob(Contract contract)
    {
        Context.Contracts.Add(contract);
        await Context.SaveChangesAsync();

        return CreatedAtAction("GetJob", new { id = contract.Id }, contract);
    }

    // DELETE: api/Contracts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        var job = await Context.Contracts.FindAsync(id);
        if (job == null)
        {
            return NotFound();
        }

        Context.Contracts.Remove(job);
        await Context.SaveChangesAsync();

        return NoContent();
    }

    private bool JobExists(Guid id)
    {
        return (Context.Contracts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
