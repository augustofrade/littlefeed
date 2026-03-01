using LittleFeed.Dto.Newsletters;
using LittleFeed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages;

public class Details(INewsletterService newsletterService,
    IArticleService articleService,
    ILogger<Details> logger)  : PageModel
{
    public NewsletterDto Newsletter { get; private set; }
    
    public async Task<IActionResult> OnGetAsync(string? slug, int page = 0)
    {
        if (slug is null)
        {
            // TODO: fix this
            logger.LogWarning("Article page accessed without a slug");
            return RedirectToPage("/Index");
        }

        var newsletter = await newsletterService.GetNewsletterBySlug(slug);
        if (newsletter is null)
        {
            logger.LogWarning("Article page accessed with invalid slug");
            return RedirectToPage("NotFound");
        }
        
        const int pageSize = 10;

        newsletter.Articles = await articleService.GetLatestArticlesFromNewsletterAsync(newsletter.Id, pageSize * page, page);
        
        Newsletter = newsletter;
        return Page();
    }
}