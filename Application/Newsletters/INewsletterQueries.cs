using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Application.Newsletters;

public interface INewsletterQueries
{
    Task<List<ListNewsletterDto>> GetNewsletters(int? amount = null);
    Task<List<ListOwnedNewsletterDto>> GetNewslettersUserCanEdit(string userId);
    Task<NewsletterDto?> GetNewsletterBySlug(string slug);
    Task<string?> GetNewsletterSlug(Guid id);
    Task<Guid?> GetNewsletterIdBySlug(string slug);
    Task<bool> ExistsBySlug(string slug);
}