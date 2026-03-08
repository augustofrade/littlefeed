using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Application.Newsletters;

public interface INewsletterCommands
{
    
    Task<NewsletterDto> CreateNewsletter(CreateNewsletterDto createDto, string ownerUserId);
    Task<Newsletter> UpdateNewsletter(Newsletter newsletter);
    Task DeleteNewsletter(string id);
}