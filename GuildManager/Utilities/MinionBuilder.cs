using GuildManager.Models;

namespace GuildManager.Utilities;

public static class MinionBuilder
{
    public static IQueryable<Minion> OfPlayer(this IQueryable<Minion> queryable, Guid bossId)
    {
        return queryable.Where(x => x.BossId == bossId);
    }

    public static Minion GenerateMinion()
    {
        return new Minion() { Name = Guid.NewGuid().ToString() };
    }
}
