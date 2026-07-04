using FluentValidation;

namespace FleetManagementSystem.Application.Commands.Trip.Create;

public class CreateTripCommandValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripCommandValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0)
            .WithMessage("Driver ID must be greater than 0");

        RuleFor(x => x.VehicleId)
            .GreaterThan(0)
            .WithMessage("Vehicle ID must be greater than 0");

        RuleFor(x => x.StartLocation)
            .NotEmpty()
            .WithMessage("Start location is required")
            .MaximumLength(200);

        RuleFor(x => x.EndLocation)
            .NotEmpty()
            .WithMessage("End location is required")
            .MaximumLength(200);

        RuleFor(x => x.Distance)
            .GreaterThan(0)
            .WithMessage("Distance must be greater than 0");

        RuleFor(x => x.CargoWeight)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Cargo weight cannot be negative");
    }
}