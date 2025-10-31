using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public class ProjectsController(IProjectService service) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

    [HttpGet]
    public Task<List<ProjectResponse>> Get(CancellationToken ct) =>
        service.GetProjectsAsync(UserId, ct);

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var p = await service.CreateProjectAsync(UserId, req, ct);
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [HttpGet("{id:guid}")]
    public Task<ProjectResponse> GetById(Guid id, CancellationToken ct) =>
        service.GetProjectAsync(id, UserId, ct);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteProjectAsync(id, UserId, ct);
        return NoContent();
    }
}
