using LittleFeed.Domain;
using Microsoft.EntityFrameworkCore;

namespace LittleFeed.Services;

public interface IUserProfileService
{
    Task<UserProfile?> GetProfileById(Guid id);
    Task<UserProfile?> GetProfileByUserId(string userId);
    Task<UserProfile?> GetProfileBySlug(string slug);
}

public class UserProfileService(ApplicationDbContext dbContext,
    ILogger<UserProfileService> logger) : IUserProfileService
{
    public Task<UserProfile?> GetProfileById(Guid id)
    {
        return dbContext.UserProfiles.FindAsync(id).AsTask();
    }

    public Task<UserProfile?> GetProfileByUserId(string userId)
    {
        return dbContext.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public Task<UserProfile?> GetProfileBySlug(string slug)
    {
        return dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Slug == slug);
    }
}