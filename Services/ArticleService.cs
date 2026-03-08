using Humanizer;
using LittleFeed.Application.Newsletters;
using LittleFeed.Common.Results;
using LittleFeed.Domain;
using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Articles;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services;

public interface IArticleService
{
    Task<List<ListArticleDto>> GetLatestArticlesAsync(int count, int skip = 0);
    Task<List<ListArticleDto>> GetLatestArticlesFromNewsletterAsync(Guid newsletterId, int count, int skip = 0);
    Task<List<ListAuthoredArticleDto>> GetLatestArticlesWrittenByUserAsync(string userId, int amount = 5);
    Task<Article?> GetArticleByIdAsync(Guid id);
    Task<Article?> GetArticleBySlugAsync(string slug);
    Task<bool> ArticleExists(string slug);
    Task<Result<ArticleDto>> CreateArticleAsDraftAsync(CreateArticleDto createDto, string userId);
    Task<Result<ArticleDto>> CreatePublishedArticleAsync(CreateArticleDto createDto, string userId);
}

public class ArticleService(ApplicationDbContext dbContext,
    INewsletterQueries newsletterQueries,
    INewsletterAccess newsletterAccess,
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

    public Task<List<ListAuthoredArticleDto>> GetLatestArticlesWrittenByUserAsync(string userId, int amount = 5)
    {
        return dbContext.Articles
            .AsNoTracking()
            .Where(a => a.AuthorId == userId)
            .Take(amount)
            .OrderBy(a => a.CreatedAt)
            .Select(a => new ListAuthoredArticleDto(a.Slug, a.Title, a.Newsletter.Slug, a.PublishDate))
            .ToListAsync();
    }

    public Task<Article?> GetArticleByIdAsync(Guid id)
    {
        return dbContext.Articles.FindAsync(id).AsTask();
    }

    public Task<Article?> GetArticleBySlugAsync(string slug)
    {
        return  dbContext.Articles.FirstOrDefaultAsync(a => a.Slug == slug);
    }

    public Task<bool> ArticleExists(string slug)
    {
        return  dbContext.Articles.AnyAsync(a => a.Slug == slug);
    }

    public Task<Result<ArticleDto>>CreateArticleAsDraftAsync(CreateArticleDto createDto, string userId)
    {
        var article = Article.Create(createDto.Title, createDto.Body, userId, createDto.NewsletterId);
        return CreateArticleAsync(article, userId);
    }

    public Task<Result<ArticleDto>> CreatePublishedArticleAsync(CreateArticleDto createDto, string userId)
    {
        var article = Article.Create(createDto.Title, createDto.Body, userId, createDto.NewsletterId);
        article.Publish();
        return CreateArticleAsync(article, userId);
    }
    
    private async Task<Result<ArticleDto>> CreateArticleAsync(Article article, string userId)
    {
        var newsletterSlug = await newsletterQueries.GetNewsletterSlug(article.NewsletterId);
        if (newsletterSlug == null)
            return Result<ArticleDto>.Failure("Newsletter not found");
        
        var canUserEditNewsletter = await newsletterAccess.CanUserEditNewsletter(article.NewsletterId, userId);
        if (!canUserEditNewsletter)
            return Result<ArticleDto>.Failure("You do not have permission to edit this newsletter");
        
        var articleExists = await ArticleExists(article.Slug);
        if(articleExists)
            return  Result<ArticleDto>.Failure($"Article with identifier \"{article.Slug}\" already exists");
        
        await dbContext.Articles.AddAsync(article);
        await dbContext.SaveChangesAsync();
    
        var dto = new ArticleDto
        {
            Title =  article.Title,
            Slug = article.Slug,
            Excerpt = article.Excerpt,
            Body = article.Body,
            IsDraft = article.IsDraft,
            NewsletterSlug =  newsletterSlug
        };
        
        return Result<ArticleDto>.Success(dto);
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