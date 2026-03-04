namespace LittleFeed.Dto.Newsletters;

public record ListOwnedNewsletterDto(Guid Id, string Name, string Slug, string Description, DateTime CreatedAt, bool IsOwner);