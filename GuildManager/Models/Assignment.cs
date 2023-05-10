namespace GuildManager.Models;

public class Assignment
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
    public int MinionId { get; set; }
    public Minion Minion { get; set; }
}
