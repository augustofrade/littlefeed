using LittleFeed.Application.Articles;
using LittleFeed.Application.Newsletters;
using LittleFeed.Common.Results;
using LittleFeed.Domain;
using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Common;
using LittleFeed.Dto.Newsletters;
using LittleFeed.Dto.Reader;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services;


public class ArticleService(ApplicationDbContext dbContext,
    INewsletterQueries newsletterQueries,
    INewsletterAccess newsletterAccess,
    ILogger<ArticleService> logger) : IArticleQueries, IArticleCommands
{
    public async Task<ListPagination<ListArticleDto>> GetLatestPublishedArticlesAsync(int amount, int page = 1)
    {
        var articles = await LatestPublishedArticlesQuery(amount, page, null)
            .Select(a => new ListArticleDto
            {
                Title =  a.Title,
                Slug =   a.Slug,
                PublishDate =  a.PublishDate!.Value,
                NewsletterSlug = a.Newsletter.Slug,
                NewsletterName = a.Newsletter.Name,
                NewsletterId = a.NewsletterId
            })
            .ToListAsync();
        
        var hasNextPage = articles.Count > amount; 
        if(articles.Count > amount)
            articles.RemoveAt(articles.Count - 1);

        return new ListPagination<ListArticleDto>(articles, amount, page, hasNextPage);
    }
    
    public async Task<ListPagination<ListArticlePreviewDto>> GetLatestPublishedArticlesFromNewsletterAsync(Guid newsletterId, int amount, int page = 1)
    {
        var articles = await LatestPublishedArticlesQuery(amount, page, newsletterId)
            .Select(a => new ListArticlePreviewDto(
                Title: a.Title,
                Slug: a.Slug,
                Excerpt:  a.Excerpt,
                PublishDate: a.PublishDate!.Value,
                Newsletter: new NewsletterIdentificationDto(a.Newsletter.Name, a.Newsletter.Slug)
                ))
            .ToListAsync();
        
        var hasNextPage = articles.Count > amount; 
        if(articles.Count > amount)
            articles.RemoveAt(articles.Count - 1);

        return new ListPagination<ListArticlePreviewDto>(articles, amount, page, hasNextPage);
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

    public Task<ArticleDetailsDto?> GetArticleBySlugAsync(string articleSlug, string newsletterSlug)
    {
        return dbContext.Articles
            .Where(a => a.Slug == articleSlug && a.Newsletter.Slug == newsletterSlug && a.PublishDate != null)
            .Select( a => new ArticleDetailsDto
            {
                Title =  a.Title,
                Body = a.Body,
                PublishDate = a.PublishDate!.Value,
                ModificationDate = a.ModifiedAt,
                Newsletter = new NewsletterIdentificationDto(a.Newsletter.Name, a.Newsletter.Slug),
                Author = dbContext.UserProfiles
                    .Where(up => up.UserId == a.AuthorId)
                    .Select(up => new UserIdentificationDto(up.DisplayName, up.Slug))
                    .FirstOrDefault()!
            })
            .FirstOrDefaultAsync();
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
            Body = article.Body,
            IsDraft = article.IsDraft,
            NewsletterSlug =  newsletterSlug
        };
        
        return Result<ArticleDto>.Success(dto);
    }

    private IQueryable<Article> LatestPublishedArticlesQuery(int amount, int page, Guid? newsletterId)
    {
        if(page > 0) page--;
        
        var query = dbContext.Articles
            .AsNoTracking()
            .Where(a => a.PublishDate != null);
        
        if(newsletterId != null)
            query = query.Where(a => a.NewsletterId == newsletterId);
        
        return query.OrderByDescending(a => a.PublishDate)
            .Skip(page * amount)
            .Take(amount + 1);

    }
}