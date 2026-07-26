
using FleetManagementSystem.Application.Commands.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;
[ApiController]
[Route("api/identity")]
public class UserController(IMediator mediator) : ControllerBase
{
  
  [HttpPost("userRoles")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> AssignRolesToUser([FromBody] AssignRolesToUserCommand command)
  {
      try
      {
           await mediator.Send(command);
            return Ok(new { message = "Role assigned to user successfully" });
      }
      catch (Exception ex)
      {
        return BadRequest(new { message = ex.Message });
      }
  }
  [HttpDelete("userRoles")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> RemoveRolesFromUser([FromBody] UnassignRolesToUserCommand command)
  {
      try
      {
           await mediator.Send(command);
            return Ok(new { message = "Role removed from user successfully" });
      }
      catch (Exception ex)
      {
        return BadRequest(new { message = ex.Message });
      }
  }
}
