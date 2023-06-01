namespace GuildManager.Models;

public class Privilege : BaseEntity
{
    public string Code { get; set; }
    public string Description { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
