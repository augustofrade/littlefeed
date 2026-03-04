using LittleFeed.Domain;
using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Articles;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services;

public interface IArticleService
{
    Task<List<ListArticleDto>> GetLatestArticlesAsync(int count, int skip = 0);
    Task<List<ListArticleDto>> GetLatestArticlesFromNewsletterAsync(Guid newsletterId, int count, int skip = 0);
    Task<Article?> GetArticleByIdAsync(Guid id);
    Task<Article?> GetArticleBySlugAsync(string slug);
    Task<ArticleDto?> CreateArticleAsync(CreateArticleDto createDto, string userId);
}

public class ArticleService(ApplicationDbContext dbContext,
    INewsletterService newsletterService,
    ILogger<ArticleService> logger) : IArticleService
{
    public Task<List<ListArticleDto>> GetLatestArticlesAsync(int count, int skip = 0)
    {
        return LatestArticlesQuery(count, skip).ToListAsync();
    }
    
    public Task<List<ListArticleDto>> GetLatestArticlesFromNewsletterAsync(Guid newsletterId, int count, int skip = 0)
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

    public async Task<ArticleDto?> CreateArticleAsync(CreateArticleDto createDto, string userId)
    {
        var newslettersUserCanEdit = await newsletterService.GetNewslettersUserCanEdit(userId);
        
        // Checks both existence of the newsletter by ID and if the user has permission to write to it.
        var newsletter = newslettersUserCanEdit.FirstOrDefault(n => n.Id == createDto.NewsletterId);
        if (newsletter == null)
        {
            // TODO: create Result type and errors
            return null;
        }
        
        var article = Article.Create(createDto.Title, createDto.Excerpt, createDto.Body, createDto.IsDraft, newsletter.Id);
        
        await dbContext.Articles.AddAsync(article);
        await dbContext.SaveChangesAsync();
    
        return new ArticleDto
        {
            Title =  article.Title,
            Slug = article.Slug,
            Excerpt = article.Excerpt,
            Body = article.Body,
            IsDraft = article.IsDraft,
            NewsletterSlug =  article.Slug
        };
    }

    private IQueryable<ListArticleDto> LatestArticlesQuery(int count = 0, int skip = 0)
    {
        return dbContext.Articles
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Skip(count * skip)
            .Where(a => !a.IsDraft)
            .Take(count)
            .Select(a => new ListArticleDto
            {
                Title =  a.Title,
                Slug =   a.Slug,
                Excerpt = a.Excerpt,
                CreatedAt = a.CreatedAt,
                NewsletterId = a.NewsletterId
            });
    }
}