using GuildManager.DAL;
using Microsoft.AspNetCore.Mvc;
using GuildManager.Models;

namespace GuildManager.Controllers
{
    [Route("api/my/contracts/")]
    [ApiController]
    public class MyContractsController : GenericController<Contract>
    {
        public MyContractsController(IUnitOfWork context) : base(context)
        {
        }

        // GET: api/my/contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();

            return Ok(await Repository.Get(c => c.PatronId == player.Id));
        }

        [HttpGet("{id}/minions")]
        public async Task<ActionResult<IEnumerable<Minion>>> GetContractMinions(Guid id)
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();

            return Ok(await UnitOfWork.GetRepository<Minion>()
                .Get(m => m.Contracts.Any(c => c.Id == id && c.PatronId == player.Id)));
        }

        // GET: api/my/contracts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(Guid id)
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();
            var contract = await Repository.GetById(id);

            if (contract == null)
            {
                return NotFound();
            }

            return contract;
        }

        // DELETE: api/my/contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> AbandonContract(Guid id)
        {
            if (!TryGetPlayer(out var player)) return Unauthorized();

            var contract = await Repository.GetById(id);
            if (contract == null || contract.PatronId != player.Id)
                return NotFound();

            if (!contract.CanBeAbandoned())
                return Problem("Contract cannot be abandoned.");

            contract.Abandon();
            await Repository.Update(contract);
            await UnitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
