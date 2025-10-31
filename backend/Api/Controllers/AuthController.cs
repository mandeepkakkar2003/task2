using Application.Abstractions;
using Application.DTOs;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserRepository users, PasswordHasher hasher, IJwtTokenService jwt) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existing = await users.GetByEmailAsync(req.Email, ct);
        if (existing is not null) throw new BadRequestException("Email already registered.");

        var u = new User { Email = req.Email, PasswordHash = hasher.Hash(req.Password) };
        u = await users.AddAsync(u, ct);
        return CreatedAtAction(nameof(Register), new { id = u.Id }, new { id = u.Id, email = u.Email });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var u = await users.GetByEmailAsync(req.Email, ct);
        if (u is null || !hasher.Verify(u.PasswordHash, req.Password))
            return Unauthorized(new ProblemDetails { Title = "Unauthorized", Detail = "Invalid credentials.", Status = 401 });

        DateTime exp;
        var token = jwt.GenerateToken(u.Id, u.Email, out exp);


        return Ok(new LoginResponse(token, exp));
    }
}
