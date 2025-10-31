// ProjectRepository.cs
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Repositories;

public class ProjectRepository(AppDbContext db) : IProjectRepository
{
    public Task<List<Project>> GetForUserAsync(Guid userId, CancellationToken ct) =>
        db.Projects.Where(p => p.UserId == userId).OrderByDescending(p => p.CreatedAt).ToListAsync(ct);

    public Task<Project?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken ct) =>
        db.Projects.Include(p => p.Tasks).SingleOrDefaultAsync(p => p.Id == id && p.UserId == userId, ct);

    public async Task<Project> AddAsync(Project project, CancellationToken ct)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync(ct);
        return project;
    }

    public async Task DeleteAsync(Project project, CancellationToken ct)
    {
        db.Projects.Remove(project);
        await db.SaveChangesAsync(ct);
    }
}
