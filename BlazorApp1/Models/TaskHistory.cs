using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// История изменений
/// </summary>
public partial class TaskHistory
{
    public int idTH { get; set; }

    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? ActionType { get; set; }

    public DateTime? ChangedAtTH { get; set; }

    public int Tasks_idT { get; set; }

    public int Users_idU { get; set; }
}
