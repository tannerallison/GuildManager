using Microsoft.EntityFrameworkCore;

namespace GuildManager.Models;

[PrimaryKey("Code")]
public class Privilege
{
    public string Code { get; set; } = null!;
    public string Description { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
