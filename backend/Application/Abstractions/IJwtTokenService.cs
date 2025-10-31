// IJwtTokenService.cs
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions;
public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email, out DateTime expiresAt);
}
