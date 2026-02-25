using LittleFeed.Domain;
using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Newsletters;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services;


public interface INewsletterService
{
    Task<List<ListNewsletterDto>> GetNewsletters();
    Task<Newsletter?> GetNewsletterById(Guid id);
    Task<NewsletterDto?> GetNewsletterBySlug(string slug);
    Task<NewsletterDto> CreateNewsletter(CreateNewsletterDto createDto);
    Task<Newsletter> UpdateNewsletter(Newsletter newsletter);
    Task DeleteNewsletter(string id);
}

public class NewsletterService(ApplicationDbContext dbContext, ILogger<NewsletterService> logger) : INewsletterService
{
    public Task<List<ListNewsletterDto>> GetNewsletters()
    {
        logger.LogInformation("Listing newsletters");
        return dbContext.Newsletters
            .Select(n => new ListNewsletterDto(n.Name, n.Slug, n.Description))
            .ToListAsync();
    }
    
    public Task<Newsletter?> GetNewsletterById(Guid id)
    {
        return dbContext.Newsletters
            .Where(n => n.Id == id)
            .Include(n => n.Articles)
            .FirstOrDefaultAsync();
    }

    public async Task<NewsletterDto?> GetNewsletterBySlug(string slug)
    {
        var newsletter = await dbContext.Newsletters
            .Where(n => n.Slug == slug)
            .Include(n => n.Articles)
            .FirstOrDefaultAsync();
        
        if(newsletter is null)
            return null;

        return new NewsletterDto
        {
            Name = newsletter.Name,
            Description = newsletter.Description,
            Slug = newsletter.Slug,
            CreatedAt = newsletter.CreatedAt,
        };
    }

    public async Task<NewsletterDto> CreateNewsletter(CreateNewsletterDto createDto)
    {
        var newsletter = new Newsletter(createDto.Name, createDto.Slug, createDto.Description);
        logger.LogInformation("Creating new newsletter");
        dbContext.Newsletters.Add(newsletter);
        await dbContext.SaveChangesAsync();
        return new NewsletterDto
        {
            Name = newsletter.Name,
            Description = newsletter.Description,
            Slug = newsletter.Slug,
            CreatedAt = newsletter.CreatedAt,
        };
    }

    public Task<Newsletter> UpdateNewsletter(Newsletter newsletter)
    {
        throw new NotImplementedException();
    }

    public Task DeleteNewsletter(string id)
    {
        throw new NotImplementedException();
    }
}