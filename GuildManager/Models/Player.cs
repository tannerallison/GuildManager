namespace GuildManager.Models;

public class Player
{
    public Player()
    {
        ApiKey = Guid.NewGuid().ToString();
    }

    public int Id { get; set; }
    public string ApiKey { get; set; }
    public string UserName { get; set; }

    public ICollection<Minion> Minions  { get; set; } = new List<Minion>();
}
