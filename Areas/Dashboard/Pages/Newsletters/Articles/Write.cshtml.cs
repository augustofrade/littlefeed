using LittleFeed.Dto.Articles;
using LittleFeed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters.Articles;

public class Write(INewsletterService newsletterService) : PageModel
{
    public required string NewsletterName { get; set; }
    public required string NewsletterSlug { get; set; }
    [BindProperty]
    public CreateArticleDto Input { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string newsletterSlug)
    {
        var newsletterName = await newsletterService.GetNewsletterNameBySlug(newsletterSlug);
        if (newsletterName == null)
        {
            return RedirectToPage("/Index");
        }
        
        NewsletterName = newsletterName;
        NewsletterSlug = newsletterSlug;
        
        return Page();
    }

    public void OnPostPublishAsync()
    {
        Input.IsDraft = false;
    }
    
    public void OnPostDraftAsync()
    {
        // Make sure it is a draft if the user manages to change it
        Input.IsDraft = true;
    }
}