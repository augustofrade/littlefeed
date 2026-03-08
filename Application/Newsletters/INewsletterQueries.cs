using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Application.Newsletters;

public interface INewsletterQueries
{
    Task<List<ListNewsletterDto>> GetNewsletters();
    Task<List<ListOwnedNewsletterDto>> GetNewslettersUserCanEdit(string userId);
    Task<NewsletterDto?> GetNewsletterBySlug(string slug);
    Task<string?> GetNewsletterSlug(Guid id);
}