namespace GuildManager.Models;

public class Job : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Minion> AssignedMinions { get; } = new List<Minion>();
}
