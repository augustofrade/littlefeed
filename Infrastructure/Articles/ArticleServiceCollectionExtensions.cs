using LittleFeed.Services;

namespace LittleFeed.Infrastructure.Articles;

public static class ArticleServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddArticleServices()
        {
            services.AddScoped<IArticleService, ArticleService>();
            return services;
        }
    }
}