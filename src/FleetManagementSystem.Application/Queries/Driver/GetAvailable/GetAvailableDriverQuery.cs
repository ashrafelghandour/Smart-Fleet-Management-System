using FleetManagementSystem.Application.DTOs.Driver;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Driver.GetAvailable;
    public sealed record GetAvailableDriversQuery : IRequest<IEnumerable<DriverResponse>>;

