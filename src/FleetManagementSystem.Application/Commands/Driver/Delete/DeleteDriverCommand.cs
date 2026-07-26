using System.ComponentModel.DataAnnotations;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Driver.Delete;

public record DeleteDriverCommand(

    [Required]
    int driverId

    ) : IRequest<bool>;
