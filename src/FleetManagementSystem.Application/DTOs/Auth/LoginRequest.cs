using System.ComponentModel.DataAnnotations;


namespace FleetManagementSystem.Application.DTOs.Auth;

public class LoginRequest
{   
    [EmailAddress]
    public string Email {get;set;}=string.Empty;
    public string Pass {get;set;} = string.Empty;
}

