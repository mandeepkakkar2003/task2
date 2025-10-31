using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security;

// Wrap ASP.NET Identity PasswordHasher<T>
public class PasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password) => _hasher.HashPassword(this, password);

    public bool Verify(string hash, string password) =>
        _hasher.VerifyHashedPassword(this, hash, password) is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
}
