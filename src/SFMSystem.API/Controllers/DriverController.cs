using FleetManagementSystem.Application.Commands.Driver.Create;
using FleetManagementSystem.Application.Commands.Driver.Delete;
using FleetManagementSystem.Application.Commands.Driver.UpdateStatus;
using FleetManagementSystem.Application.Queries.Driver.GetAlls;
using FleetManagementSystem.Application.Queries.Driver.GetAvailable;
using FleetManagementSystem.Application.Queries.Driver.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;

public class DriverController : BaseApiController{

    [HttpGet]   
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

    [HttpPut("UpdateStatus")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateDriverStatusCommand request )
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
    [HttpDelete]
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