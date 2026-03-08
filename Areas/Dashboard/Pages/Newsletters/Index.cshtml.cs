using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Newsletters;
using LittleFeed.Infrastructure.Identity;
using LittleFeed.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters;

public class Index(INewsletterService newsletterService,
    UserManager<ApplicationUser>  userManager) : PageModel
{

    public List<ListOwnedNewsletterDto> NewslettersOwnedByUser { get; set; } = [];
    public List<ListOwnedNewsletterDto> NewslettersUserCanWriteTo { get; set; } = [];
    public List<ListWrittenArticleDto> RecentlyWrittenArticles { get; set; } = [];
    
    public async Task OnGetAsync()
    {
        var currentUserId = userManager.GetUserId(User)!;
        
        var newslettersUserIsAssignedTo = await newsletterService.GetNewslettersUserCanEdit(currentUserId);

        foreach (var newsletter in newslettersUserIsAssignedTo)
        {
            if(newsletter.IsOwner)
                NewslettersOwnedByUser.Add(newsletter);
            else
                NewslettersUserCanWriteTo.Add(newsletter);  
        }
    }
}