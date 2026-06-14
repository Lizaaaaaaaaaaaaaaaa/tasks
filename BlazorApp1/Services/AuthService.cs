using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp1.Services;

public class AuthService
{
    private readonly ProtectedSessionStorage _storage;
    private readonly AppDbContext _context;

    public AuthService(
        ProtectedSessionStorage storage,
        AppDbContext context)
    {
        _storage = storage;
        _context = context;
    }

    [Required(ErrorMessage = "Email обязателен")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    public string? Password { get; set; }

    // Хеширование пароля SHA1
    private string HashPassword(string password)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha1.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();

            foreach (byte b in hash)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }

    // Проверка пользователя
    public async Task<User?> CheckUser()
    {
        // Хешируем введённый пароль
        string hashedPassword = HashPassword(Password ?? "");

        return await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Email == Email &&
                x.Password == hashedPassword);
    }

    // Сохранение пользователя в сессию
    public async Task SetUserAsync(User user)
    {
        await _storage.SetAsync("CurrentUser", user);
    }

    // Получение пользователя из сессии
    public async Task<User?> GetUserAsync()
    {
        var result = await _storage.GetAsync<User>("CurrentUser");

        return result.Success
            ? result.Value
            : null;
    }

    // Выход из аккаунта
    public async Task Logout()
    {
        await _storage.DeleteAsync("CurrentUser");
    }
}