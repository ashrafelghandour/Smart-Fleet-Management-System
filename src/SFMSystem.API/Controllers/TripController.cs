using Microsoft.AspNetCore.Mvc;
using FleetManagementSystem.Application.Commands.Trip.Create;
using SFMSystem.Application.Commands.Trip.Complete;
using FleetManagementSystem.Application.Queries.Trip.GetActive;
using FleetManagementSystem.Application.Queries.Trip.GetAll;
using FleetManagementSystem.Application.Queries.Trip.GetById;
using FleetManagementSystem.Application.Commands.Trip.AddExpense;
using Microsoft.AspNetCore.Authorization;
using FleetManagementSystem.Application.Commands.Trip.Cancel;
using FleetManagementSystem.Application.Queries.Trip.GetByVehicle;
using FleetManagementSystem.Application.Queries.Trip.GetByDriver;


namespace SFMSystem.API.Controllers;

[Authorize]
public class TripController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripCommand command)
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
    public async Task<IActionResult> CompleteTrip(int id)
    {
        try
        {
            var command = new CompleteTripCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelTrip(int id)
    {
        try
        {
            var command = new CancelTripCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpPost("{id}/expense")]
    public async Task<IActionResult> AddTripExpense(int id, [FromBody] AddTripExpenseCommand command)
    {
        try
        {
            command.TripId = id;
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        try
        {
            var query = new GetAllTripsQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(int id)
    {
        try
        {
            var query = new GetTripByIdQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveTrips()
    {
        try
        {
            var query = new GetActiveTripsQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet("driver/{driverId}")]
    public async Task<IActionResult> GetTripsByDriver(int driverId)
    {
        try
        {
            var query = new GetTripsByDriverQuery(driverId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetTripsByVehicle(int vehicleId)
    {
        try
        {
            var query = new GetTripsByVehicleQuery(vehicleId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}