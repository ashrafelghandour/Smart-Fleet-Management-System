
using System.ComponentModel.DataAnnotations;
using FleetManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FleetManagementSystem.Application.Commands.User;
public record UnassignRolesToUserCommand(
    [EmailAddress] string email,
     string roleName) : IRequest ;

public class UnassignRolesToUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<int>> roleManager
    ) : IRequestHandler<UnassignRolesToUserCommand>
{
    public async Task Handle(UnassignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var role = await roleManager.FindByNameAsync(request.roleName)
        ?? throw new Exception("Role not found");
        
        var result = await userManager.RemoveFromRoleAsync(user, request.roleName);
       
        if (!result.Succeeded)
        {
            throw new Exception("Failed to unassign role from user");
        }

       
    }

}