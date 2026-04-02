using LittleFeed.Domain.Common;
using LittleFeed.Domain.Newsletters;

namespace LittleFeed.Domain;

public class UserProfile : Entity
{
    public required string DisplayName { get; set; }
    public required string Slug { get; set; }
    public required string UserId { get; init; }
    public ICollection<ArticleComment> Comments { get; set; } = [];

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