using FleetManagementSystem.Application.Commands.Trip.AddExpense;
using FluentValidation;
using System.Data;

namespace FleetManagementSystem.Application.Commands.Trip.AddTripExpense;

   public class AddTripExpenseCommandValidator : AbstractValidator<AddTripExpenseCommand>
   {
       public AddTripExpenseCommandValidator()
       {
          RuleFor(x => x.TripId)
              .GreaterThan(0)
              .WithMessage("Trip ID must be greater than 0");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

           
       }
   }