using HireConnect.Auth.Models;

namespace HireConnect.Auth.Repository.Interface;

public interface IAuthRepository
{
    Task<UserCredential?> FindByEmailAsync(string email);
    Task<UserCredential?> FindByUserIdAsync(int userId);
    Task<bool> ExistsByEmailAsync(string email);
    Task<UserCredential> AddUserAsync(UserCredential user);
    Task<bool> DeleteByUserIdAsync(int userId);

    Task AddRefreshTokenAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetRefreshTokenAsync(string token);

    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);

    Task DeleteAllRefreshTokensForUserAsync(int userId);
}