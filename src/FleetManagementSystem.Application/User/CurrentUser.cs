
namespace FleetManagementSystem.Application.User;
    public record CurrentUser (int id , 
     string email,
     IEnumerable<string> roles
    )
{
   public bool IsInRole(string role) => roles.Contains(role);
}