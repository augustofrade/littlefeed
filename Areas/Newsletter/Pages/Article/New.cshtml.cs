using LittleFeed.Dto.Articles;
using LittleFeed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages.Article;

public class New(INewsletterService newsletterService,
    IArticleService articleService,
    ILogger<Details> logger) : PageModel
{
    [BindProperty]
    public required CreateArticleDto Input { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string? slug)
    {

        var newsletter = await newsletterService.GetNewsletterBySlug(slug);
        if (newsletter is null)
        {
            logger.LogWarning("Article page accessed with invalid slug");
            return RedirectToPage("NotFound");
        }
        
        return Page();
    }
}