using FluentValidation;

namespace FleetManagementSystem.Application.Queries.Trip.GetByDriver;

public class GetTripsByDriverQueryValidator : AbstractValidator<GetTripsByDriverQuery>
{
   public GetTripsByDriverQueryValidator()
   {
      RuleFor(x=>x.driverId).GreaterThan(0);
   }
}
