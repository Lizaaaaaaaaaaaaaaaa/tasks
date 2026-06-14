using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// Статус
/// </summary>
public partial class WorkTaskStatus
{
    public int idTS { get; set; }

    /// <summary>
    /// Название статуса
    /// </summary>
    public string NameTS { get; set; } = null!;
    public virtual ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();

}
