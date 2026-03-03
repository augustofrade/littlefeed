using LittleFeed.Domain.Common;

namespace LittleFeed.Domain;

public class UserProfile : Entity
{
    public required string DisplayName { get; set; }
    public required string Slug { get; set; }
    public required string UserId { get; init; }

    private UserProfile()
    {
        
    }

    public static UserProfile Create(string userId, string displayName)
    {
        return new UserProfile
        {
            DisplayName = displayName,
            Slug = Slugifier.Slugify(displayName),
            UserId = userId,
        };
    }
}