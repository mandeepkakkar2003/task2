// IProjectService.cs
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions;
public interface IProjectService
{
    Task<List<ProjectResponse>> GetProjectsAsync(Guid userId, CancellationToken ct);
    Task<ProjectResponse> CreateProjectAsync(Guid userId, CreateProjectRequest req, CancellationToken ct);
    Task<ProjectResponse> GetProjectAsync(Guid id, Guid userId, CancellationToken ct);
    Task DeleteProjectAsync(Guid id, Guid userId, CancellationToken ct);
}
