namespace LittleFeed.Dto.Articles;

public record ListSummarizedArticleDto : ListArticleDto
{
    public required string Excerpt { get; init; }
}