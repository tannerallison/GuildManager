using Microsoft.EntityFrameworkCore;

namespace GuildManager.Models;

[PrimaryKey("Code")]
public class Privilege
{
    public string Code { get; set; } = null!;
    public string Description { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();


    public const string MinionView = nameof(Minion) + "_view";
    public const string MinionEdit = nameof(Minion) + "_edit";
    public const string MinionCreate = nameof(Minion) + "_create";
    public const string MinionDelete = nameof(Minion) + "_delete";
    public const string MinionHire = nameof(Minion) + "_hire";
    public const string MinionFire = nameof(Minion) + "_fire";
    public const string MinionAssignJob = nameof(Minion) + "_assign_job";

    public const string PlayerView = nameof(Player) + "_view";
    public const string PlayerEdit = nameof(Player) + "_edit";
    public const string PlayerCreate = nameof(Player) + "_create";
    public const string PlayerDelete = nameof(Player) + "_delete";

    public const string JobView = nameof(Job) + "_view";
    public const string JobEdit = nameof(Job) + "_edit";
    public const string JobCreate = nameof(Job) + "_create";
    public const string JobDelete = nameof(Job) + "_delete";
    public const string JobAccept = nameof(Job) + "_accept";
    public const string JobStart = nameof(Job) + "_start";
    public const string JobQuit = nameof(Job) + "_quit";
}
