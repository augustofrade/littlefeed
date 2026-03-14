using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Dto.Articles;

public record ListArticlePreviewDto(
    string Title,
    string Slug,
    string Excerpt,
    DateTime PublishDate,
    NewsletterIdentificationDto Newsletter);