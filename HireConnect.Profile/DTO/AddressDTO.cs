
namespace HireConnect.Profile.DTO;

public class AddressDTO
{
    public string HouseNo { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Pincode { get; set; } = string.Empty;

    public AddressDTO() { }
    public AddressDTO(string houseNo, string street, string city, string state, string pincode)
    {
        HouseNo = houseNo;
        Street = street;
        City = city;
        State = state;
        Pincode = pincode;
    }
}