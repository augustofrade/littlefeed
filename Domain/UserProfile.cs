namespace LittleFeed.Domain;

public class UserProfile : Entity
{
    public string DisplayName { get; set; }
    public string Slug { get; set; }
    public string UserId { get; init; }
}