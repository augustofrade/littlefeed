using System.ComponentModel.DataAnnotations;

namespace LittleFeed.Domain;

public class Entity
{
    [Key]
    public Guid Id { get; private init; }

    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; }
}
