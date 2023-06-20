using GuildManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GuildManager.Data;

public class GMContext : DbContext
{
    public DbSet<Minion> Minions { get; set; } = null!;
    public DbSet<Contract> Contracts { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Privilege> Privileges { get; set; } = null!;

    public GMContext()
    {
    }

    public GMContext(DbContextOptions options) : base(options)
    {
    }

    public override int SaveChanges()
    {
        AddTimeStamps();
        return base.SaveChanges();
    }

    private void AddTimeStamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x is { Entity: ITimeStampedEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }

            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
}
