using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs;

public class CreateProjectRequest
{
    [Required, MinLength(3), MaxLength(100)]
    public string Title { get; set; } = default!;
    [MaxLength(500)]
    public string? Description { get; set; }
}

public record ProjectResponse(Guid Id, string Title, string? Description, DateTime CreatedAt);
