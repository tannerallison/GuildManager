namespace GuildManager.Models;

public class Assignment : BaseEntity
{
    public Guid JobId { get; set; }
    public Job Job { get; set; }
    public Guid MinionId { get; set; }
    public Minion Minion { get; set; }
}
