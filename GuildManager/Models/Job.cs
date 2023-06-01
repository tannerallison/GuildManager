using System.Text.Json.Serialization;

namespace GuildManager.Models;

public class Job : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid? PatronId { get; set; } = null;
    [JsonIgnore] public Player? Patron { get; set; } = null!;

    [JsonIgnore] public ICollection<Minion> AssignedMinions { get; } = new List<Minion>();
}
