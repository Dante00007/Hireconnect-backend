using HireConnect.Auth.Models;
using HireConnect.Auth.Repository.Interface;
using HireConnect.Auth.Service.Interface;

using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HireConnect.Auth.DTO;
using HireConnect.Auth.Exceptions;

namespace HireConnect.Auth.Services.ServiceImplement;

public class AuthServiceImpl : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _config;

    public AuthServiceImpl(IAuthRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _config = configuration;
    }

    public async Task<UserCredential> RegisterAsync(UserCredential user, string password)
    {
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.CreatedAt = DateTime.UtcNow;

        var result = await _repository.AddUserAsync(user);
        if (result == null)
        {
            throw new UserAlreadyExistsException(user.Email);
        }

        return result;
    }

    public async Task<AuthResponseDTO> LoginAsync(string email, string password)
    {
        var user = await _repository.FindByEmailAsync(email);


        if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new InvalidCredentialException();
        }

        var accessToken = GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken(user);

        await _repository.AddRefreshTokenAsync(refreshToken);

        return new AuthResponseDTO(accessToken, refreshToken.Token, user.Email, user.Role, user.UserId);
    }

    public async Task LogoutAsync(string token)
    {
        var storedToken = await _repository.GetRefreshTokenAsync(token);
        if (storedToken != null)
        {
            storedToken.IsRevoked = true;
            await _repository.UpdateRefreshTokenAsync(storedToken);
        }else
        {
            throw new TokenException("Invalid token");
        }
    }

     public async Task<AuthResponseDTO> RefreshTokenAsync(string token)
    {
        var storedToken = await _repository.GetRefreshTokenAsync(token);

        if (storedToken == null || storedToken.IsExpired || storedToken.IsRevoked || storedToken.IsUsed)
        {
            throw new TokenException("Invalid or expired refresh token");
        }

        storedToken.IsUsed = true;
        await _repository.UpdateRefreshTokenAsync(storedToken);

        var user = storedToken.User!;
        
        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken(user);

        await _repository.AddRefreshTokenAsync(newRefreshToken);

        return new AuthResponseDTO(newAccessToken, newRefreshToken.Token, user.Email, user.Role, user.UserId);
    } 
    public async Task<bool> ValidateTokenAsync(string token)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            throw new TokenException("Invalid token");
        }
    }
    public async Task<UserCredential?> GetByEmailAsync(string email) => await _repository.FindByEmailAsync(email);


    private string GenerateJwtToken(UserCredential user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"] ?? "1440")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(UserCredential user) => new RefreshToken
    {
        Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
        UserId = user.UserId,
        ExpiryDate = DateTime.UtcNow.AddDays(7),
        CreatedAt = DateTime.UtcNow
    };

   
}