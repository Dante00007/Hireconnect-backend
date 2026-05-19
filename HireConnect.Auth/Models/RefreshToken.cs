using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireConnect.Auth.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }


    public bool IsRevoked { get; set; } = false;
    public bool IsUsed { get; set; } = false;

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

    [ForeignKey("UserId")]
    public UserCredential? User { get; set; }
}
