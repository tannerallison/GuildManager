using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildManager.Data;
using GuildManager.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GuildManager.Controllers
{
    [Route("api/my/contracts/")]
    [ApiController]
    public class MyContractsController : AuthorizedController
    {
        public MyContractsController(GMContext context) : base(context)
        {
        }

        // GET: api/my/contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();

            return await Context
                .Contracts
                .Where(c => c.PatronId == player.Id)
                .ToListAsync();
        }

        [HttpGet("{id}/minions")]
        public async Task<ActionResult<IEnumerable<Minion>>> GetContractMinions(Guid id)
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();

            return await Context.Contracts
                .Where(c => c.Id == id && c.PatronId == player.Id)
                .SelectMany(c => c.AssignedMinions).ToListAsync();
        }

        // GET: api/my/contracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(Guid id)
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();
            var contract = await Context.Contracts.FindAsync(id);


            if (contract == null)
            {
                return NotFound();
            }

            return contract;
        }

        // PUT: api/my/contracts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(Guid id, Contract contract)
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
                if (!ContractExists(id))
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

        // POST: api/my/contracts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contract>> PostContract(Contract contract)
        {
            if (Context.Contracts == null)
            {
                return Problem("Entity set 'GMContext.Contracts'  is null.");
            }

            Context.Contracts.Add(contract);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetContract", new { id = contract.Id }, contract);
        }

        // DELETE: api/my/contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> AbandonContract(Guid id)
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();

            var contract = await Context.Contracts.FindAsync(id);
            if (contract == null || contract.PatronId != player.Id)
                return NotFound();

            if (!contract.CanBeAbandoned())
                return Problem("Contract cannot be abandoned.");

            contract.Abandon();
            await Context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContractExists(Guid id)
        {
            return (Context.Contracts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
