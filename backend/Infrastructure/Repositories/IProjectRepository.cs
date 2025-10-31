// IProjectRepository.cs
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;
public interface IProjectRepository
{
    Task<List<Project>> GetForUserAsync(Guid userId, CancellationToken ct);
    Task<Project?> GetByIdForUserAsync(Guid id, Guid userId, CancellationToken ct);
    Task<Project> AddAsync(Project project, CancellationToken ct);
    Task DeleteAsync(Project project, CancellationToken ct);
}
