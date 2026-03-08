using LittleFeed.Application.Articles;
using LittleFeed.Application.Newsletters;
using LittleFeed.Dto.Newsletters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages;

public class Details(INewsletterQueries newsletterQueries,
    IArticleQueries articleQueries,
    ILogger<Details> logger)  : PageModel
{
    public required NewsletterDto Newsletter { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string? slug, int page = 0)
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
        
        const int pageSize = 10;

        newsletter.Articles = await articleQueries.GetLatestArticlesFromNewsletterAsync(newsletter.Id, pageSize * page, page);
        
        Newsletter = newsletter;
        return Page();
    }
}