using System;
using System.Collections.Generic;

namespace BlazorApp1.Models;

/// <summary>
/// Пользователи
/// </summary>
public partial class User
{
    public int idU { get; set; }

    public string NameU { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int Departments_idD { get; set; }

    public sbyte IsActive { get; set; }

    public string Password { get; set; } = null!;
    public string? Role { get; set; } 
}
