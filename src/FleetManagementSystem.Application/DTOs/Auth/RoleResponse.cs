namespace FleetManagementSystem.Application.DTOs.Auth;

public class RoleResponse
{
    public string RoleName { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}