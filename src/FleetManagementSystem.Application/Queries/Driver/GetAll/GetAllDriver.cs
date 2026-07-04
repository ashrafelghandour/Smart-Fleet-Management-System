using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementSystem.Application.DTOs.Driver;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Driver.GetAlls;

public sealed record GetAllDriverQuery : IRequest<IEnumerable<DriverResponse>>;
