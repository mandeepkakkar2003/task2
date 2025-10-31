using Application.Abstractions;
using Application.DTOs;
using Domain.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/projects/{projectId:guid}/schedule")]
public class SchedulerController(IProjectService projects, ISchedulerService scheduler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ScheduleResponse>> Schedule(Guid projectId, [FromBody] List<ScheduleItem> items, CancellationToken ct)
    {
        // Ownership is enforced by attempting to fetch the project by current user inside ProjectsService.
        // We don't need the project contents, but this check is mandatory.
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);

        _ = await projects.GetProjectAsync(projectId, userId, ct); // throws 404/403 appropriately if not accessible

        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var resp = scheduler.Schedule(items);
            return Ok(resp);
        }
        catch (BadRequestException br)
        {
            return Problem(title: "Bad Request", detail: br.Message, statusCode: 400);
        }
    }
}
