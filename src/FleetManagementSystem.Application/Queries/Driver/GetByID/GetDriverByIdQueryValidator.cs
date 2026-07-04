using FluentValidation;

namespace FleetManagementSystem.Application.Queries.Driver.GetById;

public class GetDriverByIdQueryValidator : AbstractValidator<GetDriverByIdQuery>
{
    public GetDriverByIdQueryValidator()
    {
        RuleFor(x=>x.Id).GreaterThan(0);
    }
}

