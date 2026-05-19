using System.ComponentModel.DataAnnotations;

namespace HireConnect.Profile.Models;

public class Address
{
    [Key]
    public int AddressId { get; set; }
    

    [Required]
    public string HouseNo { get; set; } = string.Empty;

    [Required]
    public string Street { get; set; } = string.Empty; 

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty; 

    [Required]
    public string Pincode { get; set; } = string.Empty;
}