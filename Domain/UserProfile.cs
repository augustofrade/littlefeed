namespace LittleFeed.Domain;

public class UserProfile : Entity
{
    public required string DisplayName { get; set; }
    public string? Slug { get; set; }
    public required string UserId { get; init; }
}