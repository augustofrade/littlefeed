namespace LittleFeed.Dto.Articles;

public record ListArticleDto
{
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public required string Excerpt { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required Guid NewsletterId { get; init; }
}