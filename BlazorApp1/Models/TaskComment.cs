using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// Комментарии
/// </summary>
public partial class TaskComment
{
    public int idTC { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? CommentTC { get; set; }

    public int Tasks_idT { get; set; }

    public int Users_idU { get; set; }
}
