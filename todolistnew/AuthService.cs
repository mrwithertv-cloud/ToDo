/*
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace todolistnew;

public class AuthService
{
    private readonly IConfiguration _config;
    
    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        byte[] hashBytes = new byte[48];
        Array.Copy(salt,0,hashBytes,0,16);
        Array.Copy(hash, 0, hashBytes, 16, 32);
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyHashedPassword(string password, string savedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(savedHash);

        // Извлекаем первые 16 байт (соль)
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Хешируем введенный пароль с той же солью
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        // Сравниваем байты получившегося хэша с сохраненным
        for (int i = 0; i < 32; i++)
        {
            if (hashBytes[i + 16] != hash[i]) return false;
        }
        return true;
    }
    public string GenerateJwtToken(UserEntity user)
    {
        var secretKey = _config["Jwt:Key"] ?? "SuperSecretKey12345678901234567890!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Клеймы — данные о пользователе внутри токена
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: "ToDoApi",
            audience: "ToDoApiUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // Токен живет 8 часов
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
*/


