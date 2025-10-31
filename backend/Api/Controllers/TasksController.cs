using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Authorize]
public class TasksController(ITaskService service) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

    [HttpPost("api/projects/{projectId:guid}/tasks")]
    public async Task<ActionResult<TaskResponse>> Create(Guid projectId, CreateTaskRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var t = await service.CreateTaskAsync(projectId, UserId, req, ct);
        return CreatedAtAction(nameof(Create), new { taskId = t.Id }, t);
    }

    [HttpPut("api/tasks/{taskId:guid}")]
    public Task<TaskResponse> Update(Guid taskId, UpdateTaskRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return Task.FromResult<TaskResponse>(null!)!;
        return service.UpdateTaskAsync(taskId, UserId, req, ct);
    }

    [HttpDelete("api/tasks/{taskId:guid}")]
    public async Task<IActionResult> Delete(Guid taskId, CancellationToken ct)
    {
        await service.DeleteTaskAsync(taskId, UserId, ct);
        return NoContent();
    }

    [HttpGet("api/projects/{projectId:guid}/tasks")]
    public Task<List<TaskResponse>> List(Guid projectId, CancellationToken ct) =>
        service.GetForProjectAsync(projectId, UserId, ct);
}
