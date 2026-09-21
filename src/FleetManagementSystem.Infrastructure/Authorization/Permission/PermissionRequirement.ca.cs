using Microsoft.AspNetCore.Authorization;

namespace FleetManagementSystem.Infrastructure.Authorization.Permission;


public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission ;


}
public static class PermissionAuthorizationService
{
    public static AuthorizationPolicyBuilder RequirePermission(this AuthorizationPolicyBuilder builder, string permission)
    {
        builder.Requirements.Add(new PermissionRequirement(permission));
        return builder;
    }
}
