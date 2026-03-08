namespace LittleFeed.Dto.Articles;

public sealed record ListAuthoredArticleDto(string Slug, string Title, string NewsletterSlug, DateTime? PublishDate)
{
    public bool IsPublished => PublishDate.HasValue;
}