using LittleFeed.Common;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Newsletters;
using LittleFeed.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Dashboard.Pages.Newsletters;

public class Index(INewsletterService newsletterService,
    IArticleService articleService,
    ICurrentUser currentUser) : PageModel
{

    public List<ListOwnedNewsletterDto> NewslettersOwnedByUser { get; set; } = [];
    public List<ListOwnedNewsletterDto> NewslettersUserCanWriteTo { get; set; } = [];
    public List<ListAuthoredArticleDto> LatestWrittenArticlesByUser { get; set; } = [];
    
    public async Task OnGetAsync()
    {
        var currentUserId = currentUser.UserId!;
        
        var newslettersUserIsAssignedTo = await newsletterService.GetNewslettersUserCanEdit(currentUserId);
        LatestWrittenArticlesByUser = await articleService.GetLatestArticlesWrittenByUserAsync(currentUserId);

        foreach (var newsletter in newslettersUserIsAssignedTo)
        {
            if(newsletter.IsOwner)
                NewslettersOwnedByUser.Add(newsletter);
            else
                NewslettersUserCanWriteTo.Add(newsletter);  
        }
    }
}