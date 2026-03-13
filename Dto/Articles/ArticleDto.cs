namespace LittleFeed.Dto.Articles;

public class ArticleDto
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Body { get; set; }
    public required bool IsDraft { get; set; }
    public required string NewsletterSlug { get; set; }
}