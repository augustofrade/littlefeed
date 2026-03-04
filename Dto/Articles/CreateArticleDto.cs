using System.ComponentModel.DataAnnotations;

namespace LittleFeed.Dto.Articles;

public record CreateArticleDto
{
    [Required]
    public required string Title { get; init; }

    public string Excerpt { get; set; } = string.Empty;
    [Required]
    public required string Body { get; init; }
    [Required]
    public bool IsDraft { get; set; }
    [Required]
    public required Guid NewsletterId { get; init; }

}