using LittleFeed.Common.Results;
using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Newsletters;

namespace LittleFeed.Application.Newsletters;

public interface INewsletterCommands
{
    
    Task<Result<NewsletterIdentificationDto>> CreateNewsletter(CreateNewsletterDto createDto, string ownerUserId);
    Task<Newsletter> UpdateNewsletter(Newsletter newsletter);
    Task DeleteNewsletter(string id);
}