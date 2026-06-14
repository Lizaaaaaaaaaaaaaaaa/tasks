using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// Файлы
/// </summary>
public partial class TaskFile
{
    public int idTF { get; set; }

    public string FileName { get; set; } = null!;

    public string _FilePath { get; set; } = null!;

    public DateTime? UploadedAtTF { get; set; }

    public int Tasks_idT { get; set; }
}
