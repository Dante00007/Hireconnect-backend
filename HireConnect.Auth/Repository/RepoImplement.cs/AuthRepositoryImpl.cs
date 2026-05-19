using HireConnect.Auth.Database;
using HireConnect.Auth.Models;
using HireConnect.Auth.Repository.Interface;

using Microsoft.EntityFrameworkCore;

namespace HireConnect.Auth.Repository.RepoImplement;

public class AuthRepositoryImpl : IAuthRepository
{
    private readonly AuthDbContext _context;
    public AuthRepositoryImpl(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<UserCredential> AddUserAsync(UserCredential user)
    {
        _context.UserCredentials.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    public async Task<UserCredential?> FindByEmailAsync(string email)
    {
        return await _context.UserCredentials
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<UserCredential?> FindByUserIdAsync(int userId)
    {
        return await _context.UserCredentials.FindAsync(userId);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {

        return await _context.UserCredentials
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

   
    public async Task<bool> DeleteByUserIdAsync(int userId)
    {

        var user = await _context.UserCredentials.FindAsync(userId);
        if (user == null) return false;

        _context.UserCredentials.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User) // Eagerly load User data for the refresh flow
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllRefreshTokensForUserAsync(int userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }
}