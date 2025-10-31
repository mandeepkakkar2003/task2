using Application.Abstractions;
using Application.DTOs;
using Domain.Errors;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
namespace Application.Services;

public class TaskService(IProjectRepository projects, ITaskRepository tasks) : ITaskService
{
    public async Task<TaskResponse> CreateTaskAsync(Guid projectId, Guid userId, CreateTaskRequest req, CancellationToken ct)
    {
        var project = await projects.GetByIdForUserAsync(projectId, userId, ct) ?? throw new ForbiddenException("Forbidden.");
        var t = await tasks.AddAsync(new Domain.Entities.TaskItem {
            ProjectId = project.Id,
            Title = req.Title,
            DueDate = req.DueDate,
            IsCompleted = false
        }, ct);
        return Mappers.ToResponse(t);
    }

    public async Task<TaskResponse> UpdateTaskAsync(Guid taskId, Guid userId, UpdateTaskRequest req, CancellationToken ct)
    {
        var t = await tasks.GetByIdWithProjectAsync(taskId, userId, ct) ?? throw new NotFoundException("Task not found.");
        t.Title = req.Title;
        t.DueDate = req.DueDate;
        t.IsCompleted = req.IsCompleted;
        await tasks.UpdateAsync(t, ct);
        return Mappers.ToResponse(t);
    }

    public async Task DeleteTaskAsync(Guid taskId, Guid userId, CancellationToken ct)
    {
        var t = await tasks.GetByIdWithProjectAsync(taskId, userId, ct) ?? throw new NotFoundException("Task not found.");
        await tasks.DeleteAsync(t, ct);
    }

    public async Task<List<TaskResponse>> GetForProjectAsync(Guid projectId, Guid userId, CancellationToken ct)
    {
        // Verify ownership via project
        var project = await projects.GetByIdForUserAsync(projectId, userId, ct) ?? throw new ForbiddenException("Forbidden.");
        var list = await tasks.GetForProjectAsync(project.Id, userId, ct);
        return list.Select(Mappers.ToResponse).ToList();
    }
}
