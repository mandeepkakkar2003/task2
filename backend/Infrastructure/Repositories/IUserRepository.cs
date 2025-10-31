// IUserRepository.cs
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User> AddAsync(User user, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
}
