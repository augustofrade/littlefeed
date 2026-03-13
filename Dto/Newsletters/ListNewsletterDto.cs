using LittleFeed.Dto.Reader;

namespace LittleFeed.Dto.Newsletters;

public record ListNewsletterDto(string Name, string Slug, string Description, DateTime CreatedAt, UserIdentificationDto Owner);