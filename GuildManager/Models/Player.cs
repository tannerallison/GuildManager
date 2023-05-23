namespace GuildManager.Models;

public class Player : BaseEntity
{
    public string Username { get; set; }
    public Byte[] PasswordHash { get; set; }

    public ICollection<Minion> Minions { get; set; } = new List<Minion>();
}
