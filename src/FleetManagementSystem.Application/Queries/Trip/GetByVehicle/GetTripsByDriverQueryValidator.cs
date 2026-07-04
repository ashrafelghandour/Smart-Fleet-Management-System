using FluentValidation;

namespace FleetManagementSystem.Application.Queries.Trip.GetByVehicle;

public class GetTripsByVehicleQueryValidator : AbstractValidator<GetTripsByVehicleQuery>
{
   public GetTripsByVehicleQueryValidator()
   {
      RuleFor(x=>x.vehicleId).GreaterThan(0);
   }
}