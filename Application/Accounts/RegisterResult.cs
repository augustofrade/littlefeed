namespace LittleFeed.Application.Accounts;

public record RegisterResult(bool Succeeded, List<string> Errors)
{
    public static RegisterResult Success() => new RegisterResult(true, []);
    public static RegisterResult Failure(List<string> errors) => new RegisterResult(false, errors);
}