using LittleFeed.Domain.Newsletters;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Pagination;

namespace LittleFeed.Application.Articles;

public interface IArticleQueries
{
    Task<ListPagination<ListArticleDto>> GetLatestPublishedArticlesAsync(int amount, int page = 1);
    Task<ListPagination<ListArticlePreviewDto>> GetLatestPublishedArticlesFromNewsletterAsync(Guid newsletterId,
        int amount, int page = 1);
    Task<List<ListAuthoredArticleDto>> GetLatestArticlesWrittenByUserAsync(string userId, int amount = 5);
    Task<Article?> GetArticleByIdAsync(Guid id);
    Task<ArticleDetailsDto?> GetArticleBySlugAsync(string articleSlug, string newsletterSlug);
    Task<bool> ArticleExists(string slug);
}