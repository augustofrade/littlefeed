using LittleFeed.Dto.Newsletters;
using LittleFeed.Dto.Reader;

namespace LittleFeed.Dto.Articles;

public class ArticleDetailsDto
{
    public required string Title { get; init; }
    public required UserIdentificationDto Author { get; init; }
    public required NewsletterIdentificationDto Newsletter { get; init; }
    public required string Body { get; init; }
    public required DateTime PublishDate { get; init; }
    public required DateTime ModificationDate { get; init; }
}