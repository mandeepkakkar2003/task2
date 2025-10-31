using System;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CreateTaskRequest
{
    [Required, MinLength(1), MaxLength(200)]
    public string Title { get; set; } = default!;
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskRequest
{
    [Required, MinLength(1), MaxLength(200)]
    public string Title { get; set; } = default!;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
}

public record TaskResponse(Guid Id, Guid ProjectId, string Title, DateTime? DueDate, bool IsCompleted);

public class ScheduleItem
{
    [Required, MinLength(1), MaxLength(200)]
    public string Title { get; set; } = default!;
    [Range(1, 10000)] public int EstimatedHours { get; set; }
    public DateTime? DueDate { get; set; }
    public List<string> Dependencies { get; set; } = new();
}

public record ScheduleResponse(List<string> RecommendedOrder);
