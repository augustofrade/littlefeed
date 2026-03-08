namespace LittleFeed.Infrastructure.Auth;

public sealed record InfrastructureAuthOptions
{
    public required string LoginPath { get; init; }
    public required string AccessDeniedPath { get; init; }
    public required string LogoutPath { get; init; }
}

public static class AuthenticationServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureAuthentication(InfrastructureAuthOptions authOptions)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = authOptions.LoginPath;
                options.AccessDeniedPath = authOptions.AccessDeniedPath;
                options.LogoutPath = authOptions.LogoutPath;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
            });
            
            return services;
        }
    }
}