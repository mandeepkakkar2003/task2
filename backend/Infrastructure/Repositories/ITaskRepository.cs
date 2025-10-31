// ITaskRepository.cs
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;
public interface ITaskRepository
{
    Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct);
    Task<TaskItem?> GetByIdWithProjectAsync(Guid taskId, Guid userId, CancellationToken ct);
    Task UpdateAsync(TaskItem task, CancellationToken ct);
    Task DeleteAsync(TaskItem task, CancellationToken ct);
    Task<List<TaskItem>> GetForProjectAsync(Guid projectId, Guid userId, CancellationToken ct);
}
