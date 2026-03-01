using System.ComponentModel.DataAnnotations;

namespace LittleFeed.Dto.Articles;

public record CreateArticleDto
{
    [Required]
    public required string Title { get; init; }
    [Required]
    public required string Excerpt { get; init; }
    [Required]
    public required string Body { get; init; }
    [Required]
    public required Guid NewsletterId { get; init; }
}