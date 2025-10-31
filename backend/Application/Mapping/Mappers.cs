using Application.DTOs;
using Domain.Entities;

namespace Application.Services;

public static class Mappers
{
    public static ProjectResponse ToResponse(Project p) =>
        new(p.Id, p.Title, p.Description, p.CreatedAt);

    public static TaskResponse ToResponse(TaskItem t) =>
        new(t.Id, t.ProjectId, t.Title, t.DueDate, t.IsCompleted);
}
