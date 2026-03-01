using LittleFeed.Domain;
using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Articles;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services;

public interface IArticleService
{
    Task<List<Article>> GetLatestArticlesAsync(int count, int skip = 0);
    Task<List<Article>> GetLatestArticlesFromNewsletterAsync(Guid newsletterId, int count, int skip = 0);
    Task<Article?> GetArticleByIdAsync(Guid id);
    Task<Article?> GetArticleBySlugAsync(string slug);
    Task<Article?> CreateArticleAsync(CreateArticleDto createDto);
}

public class ArticleService(ApplicationDbContext dbContext,
    INewsletterService newsletterService,
    ILogger<ArticleService> logger) : IArticleService
{
    public Task<List<Article>> GetLatestArticlesAsync(int count, int skip = 0)
    {
        return LatestArticlesQuery(count, skip).ToListAsync();
    }
    
    public Task<List<Article>> GetLatestArticlesFromNewsletterAsync(Guid newsletterId, int count, int skip = 0)
    {
        return LatestArticlesQuery(count, skip).Where(a => a.NewsletterId == newsletterId).ToListAsync();
    }

    public Task<Article?> GetArticleByIdAsync(Guid id)
    {
        return dbContext.Articles.FindAsync(id).AsTask();
    }

    public Task<Article?> GetArticleBySlugAsync(string slug)
    {
        return  dbContext.Articles.FirstOrDefaultAsync(a => a.Slug == slug);
    }

    public async Task<Article?> CreateArticleAsync(CreateArticleDto createDto)
    {
        // TODO: create Result type and errors
        var newsletter = await newsletterService.GetNewsletterById(createDto.NewsletterId);
        if (newsletter == null)
            return null;
        
        // TODO: check if user has permission to create article in newsletter
        
        var article = Article.Create(createDto.Title, createDto.Excerpt, createDto.Body, createDto.IsDraft, newsletter);
        
        await dbContext.Articles.AddAsync(article);
        await dbContext.SaveChangesAsync();
    
        return article;
    }

    private IQueryable<Article> LatestArticlesQuery(int count = 0, int skip = 0)
    {
        return dbContext.Articles
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Skip(count * skip)
            .Where(a => !a.IsDraft)
            .Take(count);
    }
}