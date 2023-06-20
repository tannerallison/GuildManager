using GuildManager.Data;
using GuildManager.Models;

namespace GuildManager.DAL;

public interface IUnitOfWork
{
    public IRepository<T> GetRepository<T>() where T : BaseEntity;

    public int Save();

    public Task<int> SaveAsync();
}

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private GMContext context;
    private Dictionary<Type, object> repositories;

    public UnitOfWork(GMContext context)
    {
        this.context = context;
        repositories = new Dictionary<Type, object>();
    }

    public Task<int> SaveAsync()
    {
        return context.SaveChangesAsync();
    }

    public int Save()
    {
        return context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        if (repositories.Keys.Contains(typeof(T)))
        {
            return repositories[typeof(T)] as IRepository<T> ??
                   throw new InvalidOperationException("Repository not of correct type");
        }

        var repo = new Repository<T>(context);
        repositories.Add(typeof(T), repo);
        return repo;
    }
}
