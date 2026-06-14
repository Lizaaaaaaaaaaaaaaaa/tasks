using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

public partial class WorkTask
{
    public int idT { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CreatedAtT { get; set; }

    public DateTime? UpdatedAtT { get; set; }

    public int Departments_idD { get; set; }

    public int CreatedById { get; set; }

    public int TaskStatus_idTS { get; set; }

    public int TaskPriority_idTP { get; set; }

    public int AssigneeId { get; set; }

    // 🔗 НАВИГАЦИЯ
    public virtual WorkTaskStatus Status { get; set; } = null!;
    public virtual WorkTaskPriority Priority { get; set; } = null!;
}