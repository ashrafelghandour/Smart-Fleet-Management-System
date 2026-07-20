using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Driver.Delete;

public record DeleteDriverCommand(

    [Required]
    int driverId

    ) : IRequest<bool>;
