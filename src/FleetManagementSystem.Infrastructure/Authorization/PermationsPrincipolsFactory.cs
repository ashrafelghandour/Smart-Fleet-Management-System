using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FleetManagementSystem.Infrastructure.Authorization;
   
public class PermeationsUserClaimsPrincipalFactory
    : UserClaimsPrincipalFactory<AppUser, IdentityRole<int>>
{
    public PermeationsUserClaimsPrincipalFactory(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var roles = await UserManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            if (Roles.RolePermissions.TryGetValue(role, out var permissions))
            {
                foreach (var permission in permissions)
                {
                    identity.AddClaim(
                        new Claim("Permission", permission));
                }
            }
        }
       
        return identity;
    }
}

