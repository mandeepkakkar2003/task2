// UserRepository.cs
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        db.Users.SingleOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User> AddAsync(User user, CancellationToken ct)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Users.FindAsync([id], ct).AsTask();
}
