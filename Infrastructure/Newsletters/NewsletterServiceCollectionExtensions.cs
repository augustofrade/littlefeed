using LittleFeed.Application.Newsletters;
using LittleFeed.Services;

namespace LittleFeed.Infrastructure.Newsletters;

public static class NewsletterServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddNewsletterServices()
        {
            services.AddScoped<NewsletterService>();
            services.AddScoped<INewsletterQueries>(sp => sp.GetRequiredService<NewsletterService>());
            services.AddScoped<INewsletterCommands>(sp => sp.GetRequiredService<NewsletterService>());
            services.AddScoped<INewsletterAccess>(sp => sp.GetRequiredService<NewsletterService>());
            return services;
        }
    }
}