using System.Text.Json.Serialization;

namespace GuildManager.Models;

public class Minion : BaseEntity
{
    public string Name { get; set; }

    public Guid? BossId { get; set; }
    [JsonIgnore] public Player? Boss { get; set; }

    [JsonIgnore] public ICollection<Contract> Contracts { get; } = new List<Contract>();

    public bool OnAJob() => Contracts.Count > 0;
}
