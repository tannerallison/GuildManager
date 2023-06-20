using System.Linq.Expressions;
using GuildManager.Data;
using Microsoft.EntityFrameworkCore;

namespace GuildManager.DAL;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    Task<TEntity?> GetById(Guid id);
    Task<TEntity> Create(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task<bool> Delete(Guid id);
}

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    internal readonly GMContext Context;
    internal readonly DbSet<TEntity> DbSet;

    public Repository(GMContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                     StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    {
        try
        {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return entity;
    }

    public virtual async Task<bool> Delete(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        try
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
