using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FleetManagementSystem.Application.Commands.User;

public record AddNewRoleCommand(
    string roleName) : IRequest;


    public class AddNewRoleCommandHandler(
    RoleManager<IdentityRole<int>> roleManager
    ) : IRequestHandler<AddNewRoleCommand>
{
    public async Task Handle(AddNewRoleCommand request, CancellationToken cancellationToken)
    {
        var roleExists = await roleManager.RoleExistsAsync(request.roleName);
        if (roleExists)
        {
            throw new Exception("Role already exists");
        }

        var result = await roleManager.CreateAsync(new IdentityRole<int>(request.roleName));
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create role");
        }
    }

    
}