using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Articles;

namespace LittleFeed.Application.Articles;

public interface IArticleQueries
{
    Task<List<ListArticleDto>> GetLatestPublishedArticlesAsync(int amount, int skip = 0);
    Task<List<ListArticlePreviewDto>> GetLatestPublishedArticlesFromNewsletterAsync(Guid newsletterId, int amount, int skip = 0);
    Task<List<ListAuthoredArticleDto>> GetLatestArticlesWrittenByUserAsync(string userId, int amount = 5);
    Task<Article?> GetArticleByIdAsync(Guid id);
    Task<ArticleDetailsDto?> GetArticleBySlugAsync(string articleSlug, string newsletterSlug);
    Task<bool> ArticleExists(string slug);
}