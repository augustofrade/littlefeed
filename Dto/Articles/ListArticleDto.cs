using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Dto.Articles;

public record ListArticleDto
{
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public required DateTime PublishDate { get; init; }
    public required NewsletterIdentificationDto Newsletter { get; init; }
}