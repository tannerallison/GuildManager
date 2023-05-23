namespace GuildManager.Models;

public interface ITimeStampedEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}

public abstract class BaseEntity : ITimeStampedEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}
