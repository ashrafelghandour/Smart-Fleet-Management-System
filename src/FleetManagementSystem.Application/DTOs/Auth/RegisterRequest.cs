using System.ComponentModel.DataAnnotations;

namespace FleetManagementSystem.Application.DTOs.Auth;

public class RegisterRequest
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    
}
