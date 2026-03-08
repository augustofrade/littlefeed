using LittleFeed.Application.Accounts;
using LittleFeed.Common;
using LittleFeed.Infrastructure.Articles;
using LittleFeed.Infrastructure.Auth;
using LittleFeed.Infrastructure.Newsletters;

namespace LittleFeed.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructureServices()
        {
            services
                .AddNewsletterServices()
                .AddArticleServices()
                .AddScoped<ICurrentUser, CurrentUser>()
                .AddScoped<IAccountService, AccountService>();
        }
    }
}