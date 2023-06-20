using GuildManager.DAL;
using GuildManager.Models;

namespace GuildManager.Controllers;

public abstract class GenericController<T> : AuthorizedController where T : BaseEntity
{
    protected GenericController(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected IRepository<T> Repository => UnitOfWork.GetRepository<T>();

    protected Task<T?> GetEntityAsync(Guid id)
    {
        return Repository.GetById(id);
    }

    protected bool EntityExists(Guid id)
    {
        return GetEntityAsync(id).Result != null;
    }
}
