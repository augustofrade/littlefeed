using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Reader;

namespace LittleFeed.Dto.Newsletters;

public record NewsletterDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required UserIdentificationDto Author { get; init; }
    public required string Slug { get; init; }
    public required string Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public List<ListArticlePreviewDto> Articles { get; set; } = [];
}
