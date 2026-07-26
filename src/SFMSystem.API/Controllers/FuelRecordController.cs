
using FleetManagementSystem.Application.Commands.FuelRecord.Create;
using FleetManagementSystem.Application.Queries.FuelRecord.GetByVehicleId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;


[Authorize]
public class FuelRecordController : BaseApiController
{
    
    [HttpPost]
    public async Task<IActionResult> AddFuelRecord([FromBody] AddFuelRecordCommand command)
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

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetFuelRecordsByVehicle(int vehicleId)
    {
        try
        {
            var query = new GetFuelRecordByvehicleIQuery(vehicleId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}

