namespace LittleFeed.Application.Newsletters;

public interface INewsletterAccess
{
    Task<bool> CanUserEditNewsletter(Guid id, string userId);
}