using FleetManagementSystem.Application.Commands.Driver.Create;
using FleetManagementSystem.Application.Commands.Driver.Delete;
using FleetManagementSystem.Application.Commands.Driver.UpdateStatus;
using FleetManagementSystem.Application.Queries.Driver.GetAlls;
using FleetManagementSystem.Application.Queries.Driver.GetAvailable;
using FleetManagementSystem.Application.Queries.Driver.GetById;
using FleetManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;

[Authorize]
public class DriverController : BaseApiController{

    [HttpGet]   
    [Authorize( Policy = Permissions.DriversView)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var drivers = Mediator.Send(new GetAllDriverQuery());
            return HandleResult(drivers);
        }
        catch(Exception ex)
        {
           return HandleError(ex);
        }
      
      

    }
    [HttpGet("Available")]
    [Authorize( Policy = Permissions.DriversView)]
     public async Task<IActionResult> GetAvailableDriver()
    {
        try
        {
            var drivers = Mediator.Send(new GetAvailableDriversQuery());
            return HandleResult(drivers);
        }
        catch(Exception ex)
        {
           return HandleError(ex);
        }


    }


    [HttpGet("ById{id}")]   
    [Authorize( Policy = Permissions.DriversView)]
    public async Task<IActionResult> GetAll(int id)
    {
        try
        {
            var driver = Mediator.Send(new GetDriverByIdQuery());
            return HandleResult(driver);
        }
        catch(Exception ex)
        {
           return HandleError(ex);
        }


    }

    [HttpPost]
    [Authorize( Policy = Permissions.DriversCreate)]
    public async Task<IActionResult> CreateDriver([FromBody] CreateDriverCommand request )
    {
        try
        { 
             var result = Mediator.Send(request);
             return HandleResult(result);
        }
        catch(Exception ex)
        {
            return HandleError(ex);
        }
        
         
    }
    [HttpPut("{id}/status")]
    [Authorize( Policy = Permissions.DriversManageStatus)]
    public async Task<IActionResult> UpdateDriverStatus(int id, [FromBody] UpdateDriverStatusRequest request)
    {
        try
        {
            var command = new UpdateDriverStatusCommand 
            { 
                DriverId = id, 
                NewStatus = request.Status 
            };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpDelete]
    [Authorize( Policy = Permissions.DriversDelete)]
   public async Task<IActionResult> DeleteDriver([FromBody] DeleteDriverCommand request )
    {
        try
        {
             var result = Mediator.Send(request);
             return HandleResult(result);
        }
        catch(Exception ex)
        {
            return HandleError(ex);
        }
        
         
    }
}

public class UpdateDriverStatusRequest
{
    public DriverStatus Status { get; set; }
}