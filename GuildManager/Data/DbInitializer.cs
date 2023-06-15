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
                InitializePrivileges(context);
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
        var privileges = new dynamic[]
        {
            new { Code = Privilege.PlayerView, Description = "View Players", Roles = new[] { "player", "admin" } },
            new { Code = Privilege.PlayerEdit, Description = "Edit Players", Roles = new[] { "admin" } },
            new { Code = Privilege.PlayerCreate, Description = "Create Players", Roles = new[] { "admin" } },
            new { Code = Privilege.PlayerDelete, Description = "Delete Players", Roles = new[] { "admin" } },
            new { Code = Privilege.MinionView, Description = "View Minions", Roles = new[] { "player", "admin" } },
            new { Code = Privilege.MinionEdit, Description = "Edit Minions", Roles = new[] { "admin" } },
            new { Code = Privilege.MinionCreate, Description = "Create Minions", Roles = new[] { "admin" } },
            new { Code = Privilege.MinionDelete, Description = "Delete Minions", Roles = new[] { "admin" } },
            new { Code = Privilege.MinionHire, Description = "Hire Minions", Roles = new[] { "player", "admin" } },
            new { Code = Privilege.MinionFire, Description = "Fire Minions", Roles = new[] { "player", "admin" } },
            new
            {
                Code = Privilege.MinionAssignJob, Description = "Assign Minions to Job",
                Roles = new[] { "player", "admin" }
            },
            new { Code = Privilege.JobView, Description = "View Job", Roles = new[] { "player", "admin" } },
            new { Code = Privilege.JobEdit, Description = "Edit Job", Roles = new[] { "admin" } },
            new { Code = Privilege.JobCreate, Description = "Create Job", Roles = new[] { "admin" } },
            new { Code = Privilege.JobDelete, Description = "Delete Job", Roles = new[] { "admin" } },
            new { Code = Privilege.JobAccept, Description = "Accept Job", Roles = new[] { "player", "admin" } },
            new { Code = Privilege.JobStart, Description = "Start Job", Roles = new[] { "player", "admin" } },
            new { Code = Privilege.JobQuit, Description = "Quit Job", Roles = new[] { "player", "admin" } }
        };
        foreach (var privilege in privileges)
        {
            var code = privilege.Code as string;
            context.Privileges.FindOrCreate(p => p.Code == code,
                () => new Privilege { Code = code, Description = privilege.Description });
        }

        context.SaveChanges();

        var roles = new dynamic[]
        {
            new { Code = "player", Description = "Player" },
            new { Code = "admin", Description = "Administator" }
        };
        foreach (var r in roles)
        {
            var code = r.Code as string;
            var role = context.Roles.FirstOrDefault(r => r.Code == code);
            if (role == null)
            {
                role = new Role { Code = code, Description = r.Description };
                context.Roles.Add(role);
            }

            foreach (var p in privileges)
            {
                var privCode = p.Code as string;
                if (p.Roles.Contains(role.Code))
                {
                    var privilege = context.Privileges.Find(privCode);
                    role.Privileges.Add(privilege);
                }
            }

            context.SaveChanges();
        }

        context.SaveChanges();
    }
}
