namespace LittleFeed.Dto.Newsletters;

public record ListOwnedNewsletterDto(string Name, string Slug, string Description, DateTime CreatedAt, bool IsOwner);