using LittleFeed.Application.Articles;
using LittleFeed.Services;

namespace LittleFeed.Infrastructure.Articles;

public static class ArticleServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddArticleServices()
        {
            services.AddScoped<ArticleService>();
            services.AddScoped<IArticleQueries>(sp => sp.GetRequiredService<ArticleService>());
            services.AddScoped<IArticleCommands>(sp => sp.GetRequiredService<ArticleService>());
            return services;
        }
    }
}