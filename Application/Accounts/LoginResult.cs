namespace LittleFeed.Application.Accounts;

public record LoginResult(bool Succeeded, string Error)
{
    public static LoginResult Success() => new LoginResult(true, string.Empty);
    public static LoginResult Failure(string error) => new LoginResult(false, error);
    public static LoginResult NotFound() => Failure("E-mail not found");
    public static LoginResult LockedOut() => Failure("You are locked out");
    public static LoginResult RequiresEmailConfirmation() => Failure("Confirm your email before trying to sign in");
    public static LoginResult InvalidCredentials() => Failure("Incorrect e-mail and/or password");
    
}