using LittleFeed.Application.Articles;
using LittleFeed.Dto.Articles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages.Articles;

public class Index(IArticleQueries articleQueries) : PageModel
{
    public required ArticleDetailsDto Article { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string newsletterSlug, string articleSlug)
    {
        var article = await articleQueries.GetArticleBySlugAsync(articleSlug, newsletterSlug);
        if (article == null)
        {
            return RedirectToPage("/NotFound");
        }
        
        Article = article;
        return Page();
    }
}