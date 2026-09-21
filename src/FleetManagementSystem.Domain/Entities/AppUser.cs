using Microsoft.AspNetCore.Identity;

namespace FleetManagementSystem.Domain.Entities;

public class AppUser : IdentityUser<int>
{
     public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual Driver? Driver { get; set; }
}
