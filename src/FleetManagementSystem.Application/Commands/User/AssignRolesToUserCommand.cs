
using System.ComponentModel.DataAnnotations;
using FleetManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FleetManagementSystem.Application.Commands.User;
public record AssignRolesToUserCommand(
    [EmailAddress] string email,
     string roleName) : IRequest ;

public class AssignRolesToUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<int>> roleManager
    ) : IRequestHandler<AssignRolesToUserCommand>
{
    public async Task Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var role = await roleManager.FindByNameAsync(request.roleName)
        ?? throw new Exception("Role not found");
        
        var result = await userManager.AddToRoleAsync(user, request.roleName);
       
        if (!result.Succeeded)
        {
            throw new Exception("Failed to assign role to user");
        }

       
    }

 Task IRequestHandler<AssignRolesToUserCommand>.Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
 {
  return Handle(request, cancellationToken);
 }
}