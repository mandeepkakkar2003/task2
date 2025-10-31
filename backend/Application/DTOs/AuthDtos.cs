using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs;

public class RegisterRequest
{
    [Required, EmailAddress] public string Email { get; set; } = default!;
    [Required, MinLength(6), MaxLength(100)] public string Password { get; set; } = default!;
}

public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = default!;
    [Required] public string Password { get; set; } = default!;
}

public record LoginResponse(string Token, DateTime ExpiresAt);
