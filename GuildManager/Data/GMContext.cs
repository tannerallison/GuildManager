using GuildManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GuildManager.Data;

public class GMContext : DbContext
{
    public GMContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<Minion> Minions { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Player> Players { get; set;}
}
