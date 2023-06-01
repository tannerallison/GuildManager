using System.Text.Json.Serialization;

namespace GuildManager.Models;

public class Minion : BaseEntity
{
    public string Name { get; set; }

    public Guid? BossId { get; set; }
    [JsonIgnore] public Player? Boss { get; set; }

    [JsonIgnore] public ICollection<Job> Jobs { get; } = new List<Job>();

    public bool OnAJob() => Jobs.Count > 0;
}
