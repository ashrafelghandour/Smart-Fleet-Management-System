using Microsoft.AspNetCore.Mvc;
using MediatR;
using FleetManagementSystem.Application.Commands.Trip.Create;
using SFMSystem.Application.Commands.Trip.Complete;
using FleetManagementSystem.Application.Queries.Trip.GetActive;
using FleetManagementSystem.Application.Queries.Trip.GetAll;
using FleetManagementSystem.Application.Queries.Trip.GetById;

namespace SFMSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly IMediator _mediator;

    public TripController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new { Success = true, Data = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteTrip(int id)
    {
        try
        {
            var command = new CompleteTripCommand(id);
            var result = await _mediator.Send(command);
            return Ok(new { Success = true, Data = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        var query = new GetAllTripsQuery();
        var result = await _mediator.Send(query);
        return Ok(new { Success = true, Data = result });
    }

    // GET: api/trip/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripById(int id)
    {
        try
        {
            var query = new GetTripByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(new { Success = true, Data = result });
        }
        catch (Exception ex)
        {
            return NotFound(new { Success = false, Message = ex.Message });
        }
    }

    // GET: api/trip/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveTrips()
    {
        var query = new GetActiveTripsQuery();
        var result = await _mediator.Send(query);
        return Ok(new { Success = true, Data = result });
    }
}