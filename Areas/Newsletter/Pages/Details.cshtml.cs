using System.ComponentModel.DataAnnotations;
using LittleFeed.Application.Articles;
using LittleFeed.Application.Newsletters;
using LittleFeed.Common;
using LittleFeed.Dto.Articles;
using LittleFeed.Dto.Newsletters;
using LittleFeed.Dto.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleFeed.Areas.Newsletter.Pages;

public class Details(INewsletterQueries newsletterQueries,
    IArticleQueries articleQueries,
    INewsletterSubscriptionQueries  newsletterSubscriptionQueries,
    INewsletterSubscriptionCommands  newsletterSubscriptionCommands,
    ICurrentUser currentUser,
    ILogger<Details> logger)  : PageModel
{
    public required NewsletterDto Newsletter { get; set; }
    
    public required NewsletterSubscriptionButtonsDto NewsletterSubscriptionButtons { get; set; } 
    
    public NewsletterArticlePaginationDto<ListArticlePreviewDto> PublishedNewsletter { get;  private set; }
    private const int PageSize = 10;

    public async Task<IActionResult> OnGetAsync(string? slug, int pageNumber = 1)
    {
        if (slug is null)
        {
            // TODO: fix this
            logger.LogWarning("Article page accessed without a slug");
            return RedirectToPage("/Index");
        }

        var newsletter = await newsletterQueries.GetNewsletterBySlug(slug);
        if (newsletter is null)
        {
            logger.LogWarning("Article page accessed with invalid slug");
            return RedirectToPage("NotFound");
        }

        var articles = await GetLatestArticles(newsletter.Id, pageNumber);
        PublishedNewsletter = new NewsletterArticlePaginationDto<ListArticlePreviewDto>(articles, slug);
        Newsletter = newsletter;

        var isUserSubscribed = currentUser.IsAuthenticated && await newsletterSubscriptionQueries.IsUserSubscribed(currentUser.UserId!);
        NewsletterSubscriptionButtons = NewsletterSubscriptionButtonsDto.Create(newsletter.Slug, isUserSubscribed);
        
        return Page();
    }

    public async Task<IActionResult> OnGetPaginationAsync(string? newsletterSlug, int pageNumber = 1)
    {
        if (newsletterSlug is null)
            return Content("<div class='alert alert-danger'>Forbidden</div>", "text/html");
        
        var newsletter = await newsletterQueries.GetNewsletterBySlug(newsletterSlug);
        if (newsletter is null)
            return Content("<div class='alert alert-danger'>Forbidden</div>", "text/html");
        
        var pagination = await GetLatestArticles(newsletter.Id, pageNumber);
        var dto = new NewsletterArticlePaginationDto<ListArticlePreviewDto>(pagination, newsletterSlug);
        
        return Partial("Shared/_NewsletterPaginatedArticleList", dto);
    }

    public IActionResult OnPostSubscribe(string newsletterSlug)
    {
        if (!currentUser.IsAuthenticated)
        {
            return Partial("Shared/_SubscribeUnauthenticatedError",
                NewsletterSubscriptionButtonsDto.Failure(newsletterSlug));
        }
        
        return Partial("Shared/_NewsletterSubscriptionButtons", true);
    }
    
    public async Task<IActionResult> OnPostSubscribeGuestAsync(string newsletterSlug, [EmailAddress] string email)
    {
        if (currentUser.IsAuthenticated)
        {
            return Partial("Shared/_NewsletterSubscriptionButtons",
                NewsletterSubscriptionButtonsDto.Failure(newsletterSlug));
        }
        
        var newsletterId =  await newsletterQueries.GetNewsletterIdBySlug(newsletterSlug);
        var result = await newsletterSubscriptionCommands.SubscribeGuest(newsletterId!.Value, email);
        if (result.IsSuccess)
        {
            return Partial("Shared/_NewsletterSubscriptionButtons",
                NewsletterSubscriptionButtonsDto.Success(newsletterSlug));    
        }
        
        return Partial("Shared/_NewsletterSubscriptionButtons",
            NewsletterSubscriptionButtonsDto.Failure(newsletterSlug));    
    }

    private Task<ListPagination<ListArticlePreviewDto>> GetLatestArticles(Guid newsletterId, int page = 1)
    {
        return articleQueries.GetLatestPublishedArticlesFromNewsletterAsync(newsletterId, PageSize, page);
    }
}

public class NewsletterSubscriptionButtonsDto
{
    public string NewsletterSlug { get; private init; }
    public bool IsSubscribed { get; private init; }
    public bool? SuccessResult { get; private set; }

    private NewsletterSubscriptionButtonsDto(string newsletterSlug, bool isSubscribed, bool? successResult = null)
    {
        NewsletterSlug = newsletterSlug;
        IsSubscribed = isSubscribed;
        SuccessResult = successResult;
    }
    
    public static NewsletterSubscriptionButtonsDto Create(string newsletterSlug, bool isSubscribed)
        => new NewsletterSubscriptionButtonsDto(newsletterSlug, isSubscribed);
    
    public static NewsletterSubscriptionButtonsDto AlreadySubscribed(string newsletterSlug)
        => new NewsletterSubscriptionButtonsDto(newsletterSlug, true);
    
    public static NewsletterSubscriptionButtonsDto Success(string newsletterSlug)
        => new NewsletterSubscriptionButtonsDto(newsletterSlug, true, true);
    
    public static NewsletterSubscriptionButtonsDto Failure(string newsletterSlug, bool isSubscribed = false)
        => new NewsletterSubscriptionButtonsDto(newsletterSlug, isSubscribed, true);
}