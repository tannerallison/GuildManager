using Microsoft.EntityFrameworkCore;

namespace GuildManager.Utilities;

public static class Extensions
{

    public static T FindOrCreate<T>(this DbSet<T> dbSet, Func<T, bool> predicate, Func<T> create)
        where T : class
    {
        var entity = dbSet.FirstOrDefault(predicate);
        if (entity != null) return entity;
        entity = create();
        dbSet.Add(entity);
        return entity;
    }
}
