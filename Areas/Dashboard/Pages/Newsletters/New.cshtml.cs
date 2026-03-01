using LittleFeed.Dto.Newsletters;
using LittleFeed.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters;

public class New(INewsletterService newsletterService, ILogger<New> logger) : PageModel
{
    [BindProperty]
    public CreateNewsletterDto Input { get; set; }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        logger.LogInformation("Received Newsletter POST request");
        var newsletter = await newsletterService.CreateNewsletter(Input);
        return RedirectToPage("Details",  new { slug = newsletter.Slug });
    }
}