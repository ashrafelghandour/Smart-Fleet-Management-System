using FleetManagementSystem.Application.Commands.Vehicle;
using FleetManagementSystem.Application.Commands.Vehicle.Create;
using FleetManagementSystem.Application.Queries.Vehicle.GetAll;
using FleetManagementSystem.Application.Queries.Vehicle.GetAvailable;
using FleetManagementSystem.Application.Queries.Vehicle.GetById;
using FleetManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;

[Authorize]
public class VehicleController : BaseApiController
{
    [HttpPost]
    [Authorize(Policy = Permissions.VehiclesCreate)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Policy = Permissions.VehiclesManageStatus)]
    public async Task<IActionResult> UpdateVehicleStatus(int id, [FromBody] UpdateVehicleStatusRequest request)
    {
        try
        {
            var command = new UpdateVehicleStatusCommand 
            { 
                VehicleId = id, 
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


    [HttpGet]
    [Authorize(Policy = Permissions.VehiclesView)]
    public async Task<IActionResult> GetAllVehicles()
    {
        try
        {
            var query = new GetAllVehiclesQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }


    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.VehiclesView)]
    public async Task<IActionResult> GetVehicleById(int id)
    {
        try
        {
            var query = new GetVehicleByIdQuery { Id = id };
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet("available")]
    [Authorize(Policy = Permissions.VehiclesView)]
    public async Task<IActionResult> GetAvailableVehicles()
    {
        try
        {
            var query = new GetAvailableVehiclesQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}


public class UpdateVehicleStatusRequest
{
    public VehicleStatus Status { get; set; }
}