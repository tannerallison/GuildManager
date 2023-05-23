using GuildManager.Models;
using GuildManager.Services;
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
                var authenticationService = services.GetRequiredService<IAuthenticationService>();

                Initialize(context, authenticationService);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }

    public static void Initialize(GMContext context, IAuthenticationService authenticationService)
    {
        context.Database.EnsureCreated();

        if (context.Players.Any())
            return;

        var players = new AuthenticationRequest[]
        {
            new() { Username = "John", Password = "john" },
            new() { Username = "Jim", Password = "jim" },
            new() { Username = "Jamie", Password = "jamie" },
            new() { Username = "Jenny", Password = "jenny" },
            new() { Username = "Jack", Password = "jack" },
            new() { Username = "Jared", Password = "jared" }
        };
        foreach (AuthenticationRequest player in players)
        {
            authenticationService.Register(player);
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
