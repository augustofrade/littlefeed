using LittleFeed.Application.Articles;
using LittleFeed.Common;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Pages;

public class Featured(IArticleQueries articleQueries) : PageModel
{
    public required ListPagination<ListArticleDto> ArticlePagination { get; set; }
    
    private const int PageSize = 10;
    
    public async Task OnGetAsync()
    {
        ArticlePagination = await articleQueries.GetLatestPublishedArticlesAsync(PageSize);
    }
}