using System.ComponentModel.DataAnnotations;

namespace GuildManager.Models;

public class Minion
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Player Boss { get; set; }

    public ICollection<Job> Jobs { get; } = new List<Job>();
}
