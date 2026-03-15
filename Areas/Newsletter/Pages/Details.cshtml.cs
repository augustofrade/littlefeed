using LittleFeed.Application.Articles;
using LittleFeed.Application.Newsletters;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Common;
using LittleFeed.Dto.Newsletters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages;

public class Details(INewsletterQueries newsletterQueries,
    IArticleQueries articleQueries,
    ILogger<Details> logger)  : PageModel
{
    public required NewsletterDto Newsletter { get; set; }
    public NewsletterArticlePaginationDto PublishedNewsletterArticles { get;  private set; }
    private const int PageSize = 10;

    public async Task<IActionResult> OnGetAsync(string? slug, int pageNumber = 1)
    {
        if (slug is null)
        {
            // TODO: fix this
            logger.LogWarning("Article page accessed without a slug");
            return RedirectToPage("/Index");
        }

        var newsletter = await newsletterQueries.GetNewsletterBySlug(slug);
        if (newsletter is null)
        {
            logger.LogWarning("Article page accessed with invalid slug");
            return RedirectToPage("NotFound");
        }
        
        PublishedNewsletterArticles = new NewsletterArticlePaginationDto(await GetLatestArticles(newsletter.Id, pageNumber), slug);
        Newsletter = newsletter;
        
        return Page();
    }

    public async Task<IActionResult> OnGetPaginationAsync(string? newsletterSlug, int pageNumber = 1)
    {
        
        if (newsletterSlug is null)
            return Content("<div class='alert alert-error'>Forbidden</div>", "text/html");
        
        var newsletter = await newsletterQueries.GetNewsletterBySlug(newsletterSlug);
        if (newsletter is null)
            return Content("<div class='alert alert-error'>Forbidden</div>", "text/html");
        
        var pagination = await GetLatestArticles(newsletter.Id, pageNumber);
        var dto = new NewsletterArticlePaginationDto(pagination, newsletterSlug);
        
        return Partial("Shared/_NewsletterPaginatedArticleList", dto);
    }

    private Task<ListPagination<ListArticlePreviewDto>> GetLatestArticles(Guid newsletterId, int page = 1)
    {
        return articleQueries.GetLatestPublishedArticlesFromNewsletterAsync(newsletterId, PageSize, page);
    }
}

public record NewsletterArticlePaginationDto(ListPagination<ListArticlePreviewDto> Pagination, string NewsletterSlug);