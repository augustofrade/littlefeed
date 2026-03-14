using LittleFeed.Application.Articles;
using LittleFeed.Application.Newsletters;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Newsletters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Pages;

public class Index(INewsletterQueries newsletterQueries, IArticleQueries articleQueries) : PageModel
{
    public List<ListNewsletterDto> NewsletterList { get; private set; } = [];
    public List<ListArticleDto> ArticleList { get; private set; } = [];
    
    public async Task OnGetAsync()
    {
        NewsletterList = await newsletterQueries.GetNewsletters();
        ArticleList = await articleQueries.GetLatestPublishedArticlesAsync(10);
    }
}