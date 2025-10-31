// TaskRepository.cs
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Repositories;

public class TaskRepository(AppDbContext db) : ITaskRepository
{
    public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct)
    {
        db.Tasks.Add(task);
        await db.SaveChangesAsync(ct);
        return task;
    }

    public Task<TaskItem?> GetByIdWithProjectAsync(Guid taskId, Guid userId, CancellationToken ct) =>
        db.Tasks.Include(t => t.Project)
                .SingleOrDefaultAsync(t => t.Id == taskId && t.Project!.UserId == userId, ct);

    public async Task UpdateAsync(TaskItem task, CancellationToken ct)
    {
        db.Tasks.Update(task);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TaskItem task, CancellationToken ct)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync(ct);
    }

    public Task<List<TaskItem>> GetForProjectAsync(Guid projectId, Guid userId, CancellationToken ct) =>
        db.Tasks.Where(t => t.ProjectId == projectId && t.Project!.UserId == userId)
                .OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate)
                .ToListAsync(ct);
}
