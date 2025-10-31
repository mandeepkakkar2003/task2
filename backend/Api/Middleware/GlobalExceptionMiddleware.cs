using System.Net;
using System.Text.Json;
using Domain.Errors;

namespace Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblem(ctx, ex);
        }
    }

    private static Task WriteProblem(HttpContext ctx, Exception ex)
    {
        int status = (int)HttpStatusCode.InternalServerError;
        string title = "An unexpected error occurred.";
        string detail = "";

        switch (ex)
        {
            case Domain.Errors.NotFoundException nf:
                status = StatusCodes.Status404NotFound; title = "Not Found"; detail = nf.Message; break;
            case ForbiddenException fb:
                status = StatusCodes.Status403Forbidden; title = "Forbidden"; detail = fb.Message; break;
            case BadRequestException br:
                status = StatusCodes.Status400BadRequest; title = "Bad Request"; detail = br.Message; break;
        }

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/problem+json";
        var problem = new
        {
            type = "about:blank",
            title,
            status,
            detail
        };
        return ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
