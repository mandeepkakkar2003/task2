using Application.Abstractions;
using Application.DTOs;
using Domain.Entities;
using Domain.Errors;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Services;

public class ProjectService(IProjectRepository projects) : IProjectService
{
    public async Task<List<ProjectResponse>> GetProjectsAsync(Guid userId, CancellationToken ct)
        => (await projects.GetForUserAsync(userId, ct)).Select(Mappers.ToResponse).ToList();

    public async Task<ProjectResponse> CreateProjectAsync(Guid userId, CreateProjectRequest req, CancellationToken ct)
    {
        var p = new Project { UserId = userId, Title = req.Title, Description = req.Description };
        p = await projects.AddAsync(p, ct);
        return Mappers.ToResponse(p);
    }

    public async Task<ProjectResponse> GetProjectAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var p = await projects.GetByIdForUserAsync(id, userId, ct) ?? throw new NotFoundException("Project not found.");
        return Mappers.ToResponse(p);
    }

    public async Task DeleteProjectAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var p = await projects.GetByIdForUserAsync(id, userId, ct) ?? throw new NotFoundException("Project not found.");
        await projects.DeleteAsync(p, ct);
    }
}
