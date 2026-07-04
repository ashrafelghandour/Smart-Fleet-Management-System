using FluentValidation;

namespace FleetManagementSystem.Application.Commands.Vehicle.Create;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor( x=>x.Year).GreaterThan( DateTime.UtcNow.Year).WithMessage("The Year in Future Please Enter Correct Year");
    }
}
