using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// Отдел
/// </summary>
public partial class Department
{
    public int idD { get; set; }

    /// <summary>
    /// Название отдела
    /// </summary>
    public string Name { get; set; } = null!;
}
