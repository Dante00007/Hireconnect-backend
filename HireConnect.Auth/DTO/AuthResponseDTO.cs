
namespace HireConnect.Auth.DTO;
public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int UserId { get; set; }

    public AuthResponseDTO(string token, string refreshToken, string email, string role, int userId)
    {
        Token = token;
        RefreshToken = refreshToken;
        Email = email;
        Role = role;
        UserId = userId;
    }

}
