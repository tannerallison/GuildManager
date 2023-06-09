﻿using System.Text.Json.Serialization;

namespace GuildManager.Models;

public class Contract : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public Guid? PatronId { get; set; } = null;
    [JsonIgnore] public Player? Patron { get; set; } = null!;

    [JsonIgnore] public ICollection<Minion> AssignedMinions { get; } = new List<Minion>();

    /// <summary>
    /// The contract is in a state where it can be abandoned by the patron
    /// </summary>
    /// <returns></returns>
    public bool CanBeAbandoned() => PatronId != null;

    public void Abandon()
    {
        AssignedMinions.Clear();
        PatronId = null;
    }

}
