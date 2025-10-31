using System;

namespace Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = default!;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public Project? Project { get; set; }
}
