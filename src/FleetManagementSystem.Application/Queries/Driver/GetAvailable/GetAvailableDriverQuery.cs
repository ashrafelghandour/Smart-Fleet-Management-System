using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementSystem.Application.DTOs.Driver;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Driver.GetAvailable;
    public sealed record GetAvailableDriversQuery : IRequest<IEnumerable<DriverResponse>>;

