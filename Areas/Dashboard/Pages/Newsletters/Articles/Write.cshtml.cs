using LittleFeed.Application.Articles;
using LittleFeed.Application.Newsletters;
using LittleFeed.Common;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Newsletters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters.Articles;

public class Write(INewsletterQueries newsletterQueries,
    INewsletterAccess newsletterAccess,
    IArticleCommands articleCommands,
    ICurrentUser currentUser): PageModel
{
    public required NewsletterDto CurrentNewsletter { get; set; }
    
    [BindProperty]
    public CreateArticleDto Input { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string newsletterSlug)
    {
        var newsletter = await newsletterQueries.GetNewsletterBySlug(newsletterSlug);
        if (newsletter == null)
            return RedirectToPage("/Index");

        var canUserEdit = await newsletterAccess.CanUserEditNewsletter(newsletter.Id, currentUser.UserId!);
        if(!canUserEdit)
            return RedirectToPage("/Index");
        
        CurrentNewsletter = newsletter;
        
        return Page();
    }

    public async Task<IActionResult> OnPostPublishAsync()
    {
        if (!ModelState.IsValid)
            return SubmitError(ModelStateErrors());

        var result = await articleCommands.CreatePublishedArticleAsync(Input, currentUser.UserId!);
        if(!result.IsSuccess)
            return SubmitError(result.Error);

        return Partial("Shared/_ArticleSubmitSuccess", result.Data);
    }
    
    public async Task<IActionResult> OnPostDraftAsync()
    {
        if (!ModelState.IsValid)
            return SubmitError(ModelStateErrors());

        var result = await articleCommands.CreateArticleAsDraftAsync(Input, currentUser.UserId!);
        if(!result.IsSuccess)
            return SubmitError(result.Error);

        return Partial("Shared/_ArticleSubmitSuccess", result.Data);
    }
    
    private PartialViewResult SubmitError(params List<string> errors)
    {
        return Partial("Shared/_ArticleSubmitError", errors);
    }

    private List<string> ModelStateErrors()
    {
        return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
    }
}