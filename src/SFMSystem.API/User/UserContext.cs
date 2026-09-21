using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FleetManagementSystem.Application.Interface;


namespace FleetManagementSystem.Application.User;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{

    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null )
        {
           throw new InvalidOperationException("User Context it  is not Present.");
        }
        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        var rolesClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

      

        return new CurrentUser(
            int.Parse(idClaim.Value??default),
            emailClaim.Value??default,
            rolesClaims
        );
    }
       
}