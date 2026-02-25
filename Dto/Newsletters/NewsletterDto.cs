namespace LittleFeed.Dto.Newsletters;

public record NewsletterDto
{
    public required string Name { get; init; }
    public required string Slug { get; init; }
    public required string Description { get; init; }
    public required DateTime CreatedAt { get; init; }
}
