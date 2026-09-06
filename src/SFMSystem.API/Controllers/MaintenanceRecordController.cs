using FleetManagementSystem.Application.Commands.MaintenanceRecord.Add;
using FleetManagementSystem.Application.Commands.MaintenanceRecord.Update;
using FleetManagementSystem.Application.Queries.MaintenanceRecord;
using FleetManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;


[Authorize]
public class MaintenanceRecordController : BaseApiController
{
    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Driver+ "," + Roles.Manager)]
    public async Task<IActionResult> AddMaintenanceRecord([FromBody] AddMaintenanceRecordCommand command)
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

    [HttpPut("{id}/complete")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Driver+ "," + Roles.Manager)]

    public async Task<IActionResult> CompleteMaintenance(int id)
    {
        try
        {
            var command = new CompleteMaintenanceCommand { MaintenanceId = id };
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet("vehicle/{vehicleId}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Driver+ "," + Roles.Manager)]

    public async Task<IActionResult> GetMaintenanceRecordsByVehicle(int vehicleId)
    {
        try
        {
            var query = new GetMaintenanceRecordsByVehicleQuery(vehicleId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}