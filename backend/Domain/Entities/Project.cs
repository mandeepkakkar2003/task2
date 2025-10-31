using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
