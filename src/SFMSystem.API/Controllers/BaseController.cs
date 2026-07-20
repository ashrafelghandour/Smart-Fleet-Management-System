using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFMSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public  abstract class BaseApiController : ControllerBase
{    
    
    private IMediator? _mediator;
    private ILogger? _logger;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<BaseApiController>>();

    protected IActionResult HandleResult<T>(T? result, string? errorMessage = null)
    {
        if (result == null)
            return NotFound(new { Success = false, Message = errorMessage ?? "Resource not found" });

        return Ok(new { Success = true, Data = result });
    }

    protected IActionResult HandleError(Exception ex)
    {
        Logger.LogError(ex, "An error occurred");
        return BadRequest(new { Success = false, Message = ex.Message });
    }
}