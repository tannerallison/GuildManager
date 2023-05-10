using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GuildManager.Models;

public class Minion
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int? BossId { get; set; }

    [JsonIgnore]
    public Player? Boss { get; set; }

    public ICollection<Job> Jobs { get; } = new List<Job>();
}
