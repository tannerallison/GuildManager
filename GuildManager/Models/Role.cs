namespace GuildManager.Models;

public class Role : BaseEntity
{
    public string Code { get; set; }
    public string Description { get; set; }
    public ICollection<Privilege> Privileges { get; set; } = new List<Privilege>();
    public ICollection<Player> Players { get; set; } = new List<Player>();
}
