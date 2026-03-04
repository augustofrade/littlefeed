using Humanizer;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Newsletters;
using LittleFeed.Infrastructure.Identity;
using LittleFeed.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters.Articles;

public class Write(INewsletterService newsletterService,
    IArticleService articleService,
    UserManager<ApplicationUser> userManager) : PageModel
{
    public required NewsletterDto CurrentNewsletter { get; set; }
    [BindProperty]
    public CreateArticleDto Input { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string newsletterSlug)
    {
        var newsletter = await newsletterService.GetNewsletterBySlug(newsletterSlug);
        if (newsletter == null)
        {
            return RedirectToPage("/Index");
        }
        CurrentNewsletter = newsletter;
        
        return Page();
    }

    public void OnPostPublishAsync()
    {
        Input.IsDraft = false;
    }
    
    public async Task<IActionResult> OnPostDraftAsync()
    {
        Input.Excerpt = Input.Body.Truncate(100);
        if (!ModelState.IsValid)
        {
            var errors =  ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return SubmitError(errors.ToList());
        }
        
        // Make sure it is a draft if the user manages to change it
        Input.IsDraft = true;
        var currentUserId = userManager.GetUserId(User);
        var result = await articleService.CreateArticleAsync(Input, currentUserId!);
        
        if(result == null)
            return SubmitError(["Unknown error"]);

        return Partial("Shared/_ArticleSubmitSuccess", result);
    }
    
    private PartialViewResult SubmitError(List<string> errors)
    {
        return Partial("Shared/_ArticleSubmitError", errors);
    }
}