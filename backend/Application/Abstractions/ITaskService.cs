// ITaskService.cs
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions;
public interface ITaskService
{
    Task<TaskResponse> CreateTaskAsync(Guid projectId, Guid userId, CreateTaskRequest req, CancellationToken ct);
    Task<TaskResponse> UpdateTaskAsync(Guid taskId, Guid userId, UpdateTaskRequest req, CancellationToken ct);
    Task DeleteTaskAsync(Guid taskId, Guid userId, CancellationToken ct);
    Task<List<TaskResponse>> GetForProjectAsync(Guid projectId, Guid userId, CancellationToken ct);
}
