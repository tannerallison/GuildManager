using GuildManager.Models;
using GuildManager.Utilities;

namespace GuildManager.Data;

public static class DbInitializer
{
    public static void CreateDbIfNotExists(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<GMContext>();
                Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }

    public static void Initialize(GMContext context)
    {
        context.Database.EnsureCreated();

        if (context.Players.Any())
            return;

        var players = new Player[]
        {
            new() { UserName = "John" },
            new() { UserName = "Jim" },
            new() { UserName = "Jamie" },
            new() { UserName = "Jenny" },
            new() { UserName = "Jack" },
            new() { UserName = "Jared" }
        };
        foreach (Player player in players)
        {
            context.Players.Add(player);
        }

        context.SaveChanges();

        foreach (Player player in context.Players)
        {
            for (int i = 0; i < new Random().Next(5); i++)
            {
                var minion = MinionBuilder.GenerateMinion();
                minion.BossId = player.Id;
                context.Minions.Add(minion);
            }
        }
        context.SaveChanges();
    }
}
