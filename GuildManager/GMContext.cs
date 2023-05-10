using GuildManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GuildManager;

public class GMContext : DbContext
{
    public GMContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<Minion> Minions { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
}
