namespace LittleFeed.Dto.Articles;

public class ArticleDto
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public required string Excerpt { get; set; }
    public required string Body { get; set; }
    public Guid NewsletterId { get; private set; }
}