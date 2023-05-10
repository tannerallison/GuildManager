using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager;
using GuildManager.Filters;
using GuildManager.Models;

namespace GuildManager.Controllers
{
    [ServiceFilter(typeof(ApiKeyAuthAttribute))]
    [Route("api/[controller]")]
    [ApiController]
    public class MinionController : ControllerBase
    {
        private readonly GMContext _context;

        public MinionController(GMContext context)
        {
            _context = context;
        }

        // GET: api/Minion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Minion>>> GetMinions()
        {
            if (_context.Minions == null)
            {
                return NotFound();
            }

            return await _context.Minions.ToListAsync();
        }

        // GET: api/Minion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Minion>> GetMinion(int id)
        {
            if (_context.Minions == null)
            {
                return NotFound();
            }

            var minion = await _context.Minions.FindAsync(id);

            if (minion == null)
            {
                return NotFound();
            }

            return minion;
        }

        // PUT: api/Minion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMinion(int id, Minion minion)
        {
            if (id != minion.Id)
            {
                return BadRequest();
            }

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

        // POST: api/Minion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Minion>> PostMinion(Minion minion)
        {
            if (_context.Minions == null)
            {
                return Problem("Entity set 'GMContext.Minions'  is null.");
            }

            _context.Minions.Add(minion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMinion", new { id = minion.Id }, minion);
        }

        // DELETE: api/Minion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMinion(int id)
        {
            if (_context.Minions == null)
            {
                return NotFound();
            }

            var minion = await _context.Minions.FindAsync(id);
            if (minion == null)
            {
                return NotFound();
            }

            _context.Minions.Remove(minion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MinionExists(int id)
        {
            return (_context.Minions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
