namespace LittleFeed.Dto.Articles;

public sealed record ListWrittenArticleDto(string Slug, string Title, string NewsletterSlug, DateTime? PublishDate);