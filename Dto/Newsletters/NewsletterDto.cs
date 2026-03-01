using LittleFeed.Dto.Articles;

namespace LittleFeed.Dto.Newsletters;

public record NewsletterDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Slug { get; init; }
    public required string Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public List<ListArticleDto> Articles { get; set; } = [];
}
