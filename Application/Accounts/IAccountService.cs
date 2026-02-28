namespace LittleFeed.Application.Accounts;

public interface IAccountService
{
    Task<RegisterResult> RegisterAsync(string email, string password);
    Task<LoginResult> SignInAsync(string email, string password);
}