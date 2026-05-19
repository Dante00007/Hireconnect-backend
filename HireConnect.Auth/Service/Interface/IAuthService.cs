using HireConnect.Auth.DTO;
using HireConnect.Auth.Models;

namespace HireConnect.Auth.Service.Interface;

public interface IAuthService
{
    Task<UserCredential> RegisterAsync(UserCredential user, string password);
    Task<AuthResponseDTO> LoginAsync(string email, string password);
    Task LogoutAsync(string token);
    Task<AuthResponseDTO> RefreshTokenAsync(string token);

    Task<bool> ValidateTokenAsync(string token);
    Task<UserCredential?> GetByEmailAsync(string email);
}