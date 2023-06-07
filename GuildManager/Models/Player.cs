using System.Text.Json.Serialization;

namespace GuildManager.Models;

public class Player : BaseEntity
{
    public string Username { get; set; }
    [JsonIgnore] public Byte[] PasswordHash { get; set; }

    [JsonIgnore] public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    [JsonIgnore] public ICollection<Minion> Minions { get; set; } = new List<Minion>();
    [JsonIgnore] public ICollection<Role> Roles { get; set; } = new List<Role>();
}
