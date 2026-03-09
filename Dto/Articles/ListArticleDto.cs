namespace LittleFeed.Dto.Articles;

public record ListArticleDto
{
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public required DateTime PublishDate { get; init; }
    public required string NewsletterSlug { get; init; }
    public required string NewsletterName { get; init; }
    public required Guid  NewsletterId { get; init; }
}