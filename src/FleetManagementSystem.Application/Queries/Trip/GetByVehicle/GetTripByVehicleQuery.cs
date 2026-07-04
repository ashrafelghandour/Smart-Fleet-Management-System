using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetByVehicle;
 public sealed record  GetTripsByVehicleQuery(int vehicleId):IRequest<IEnumerable<TripResponse>>;
