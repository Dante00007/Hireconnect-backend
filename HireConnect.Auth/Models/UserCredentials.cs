using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HireConnect.Auth.Models;


// This class will represent the user in database.
public class UserCredential
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty; 


    public string? PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } = "Candidate"; 

    [Required]
    public string Provider { get; set; } = "Local"; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
}