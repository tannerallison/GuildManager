using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;

namespace GuildManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : AuthenticatedController
{
    public JobsController(GMContext context) : base(context)
    {
    }

    // GET: api/Jobs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
    {
        return await Context.Jobs.Where(j => j.PatronId == null).ToListAsync();
    }

    // GET: api/Jobs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Job>> GetJob(Guid id)
    {
        var job = await Context.Jobs.FindAsync(id);

        if (job == null)
        {
            return NotFound();
        }

        return job;
    }

    // PUT: api/Jobs/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJob(Guid id, Job job)
    {
        if (id != job.Id)
        {
            return BadRequest();
        }

        Context.Entry(job).State = EntityState.Modified;

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

    // POST: api/Jobs
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Job>> PostJob(Job job)
    {
        Context.Jobs.Add(job);
        await Context.SaveChangesAsync();

        return CreatedAtAction("GetJob", new { id = job.Id }, job);
    }

    // DELETE: api/Jobs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        var job = await Context.Jobs.FindAsync(id);
        if (job == null)
        {
            return NotFound();
        }

        Context.Jobs.Remove(job);
        await Context.SaveChangesAsync();

        return NoContent();
    }

    private bool JobExists(Guid id)
    {
        return (Context.Jobs?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
