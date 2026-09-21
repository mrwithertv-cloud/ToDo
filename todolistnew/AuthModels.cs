/*using System.ComponentModel.DataAnnotations;

namespace todolistnew;
public class UserEntity
{
    public int [key] UserId { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
}

public record AuthRequest(
    [Required(ErrorMessage = "Имя пользователя обязательно")] string Username,
    [Required(ErrorMessage = "Пароль обязателен")] string Password
);
public record AuthResponse(string AccessToken);*/