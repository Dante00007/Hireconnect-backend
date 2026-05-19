using Microsoft.AspNetCore.Mvc;
using HireConnect.Auth.Models;
using HireConnect.Auth.Service.Interface;
using HireConnect.Auth.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HireConnect.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO request)
    {
        var user = new UserCredential
        {
            Email = request.Email,
            Role = request.Role,
            Provider = "Local"
        };


        var result = await _authService.RegisterAsync(user, request.Password);

        return Ok(new { message = "User registered successfully" });

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO request)
    {
        var response = await _authService.LoginAsync(request.Email, request.Password);

        if (response == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        SetRefreshTokenCookie(response.RefreshToken);

        return Ok(new
        {
            token = response.Token,
            email = response.Email,
            role = response.Role,
            userId = response.UserId
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken);
        }

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Ok(new { message = "Logged out successfully" });
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized();

        var response = await _authService.RefreshTokenAsync(refreshToken);
        if (response == null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token" });
        }

        SetRefreshTokenCookie(response.RefreshToken);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> ValidateToken()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(new { email, role, userId });
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}

/*
{
	"email" : "ks1658759@gmail.com",
	"password": "Krishna@2804",
	"role" : "Recruiter"
}
*/

/*
{
	"email" : "gamingstrkr@gmail.com",
	"password": "Candidate@2804",
	"role" : "Candidate"
}
*/