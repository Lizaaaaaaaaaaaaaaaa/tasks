using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// Приоритеты
/// </summary>
public partial class WorkTaskPriority
{
    public int idTP { get; set; }

    public string NameTP { get; set; } = null!;

    public string? Level { get; set; }
    public virtual ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
}
