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


    public static void InitializePrivileges(GMContext context)
    {
        var privileges = new Privilege[]
        {
            new() { Code = $"{nameof(Player)}_create", Description = "Create Players" },
            new() { Code = $"{nameof(Player)}_delete", Description = "Delete Players" },
            new() { Code = $"{nameof(Minion)}_create", Description = "Create Minions" },
            new() { Code = $"{nameof(Minion)}_delete", Description = "Delete Minions" },
            new() { Code = $"{nameof(Minion)}_update", Description = "Update Minions" },
            new() { Code = $"{nameof(Minion)}_hire", Description = "Hire Minions" },
            new() { Code = $"{nameof(Job)}_create", Description = "Create Jobs" },
            new() { Code = $"{nameof(Job)}_delete", Description = "Delete Jobs" }
        };
        foreach (var privilege in privileges)
        {
            if (context.Privileges.Any(p => p.Code == privilege.Code))
                continue;
            context.Privileges.Add(privilege);
        }
        context.SaveChanges();

        var roles = new Role[]
        {
            new() { Code = "player", Description = "Player" },
            new() { Code = "admin", Description = "Administator" }
        };
        foreach (var role in roles)
        {
            if (context.Roles.Any(r => r.Code == role.Code))
                continue;
            context.Roles.Add(role);
        }
    }
}
