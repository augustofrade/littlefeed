namespace LittleFeed.Application.Accounts;

public interface IAccountService
{
    Task<RegisterResult> RegisterAsync(string email, string password);
}