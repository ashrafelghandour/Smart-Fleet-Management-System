using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace FleetManagementSystem.Application.ValidationBehavior;
   
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
 : IPipelineBehavior<TRequest, TResponse>
 where TRequest : notnull
 {
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)

  {
      if (!validators.Any())
        return await next();
  


   var context = new ValidationContext<TRequest>(request);

   var result = await Task.WhenAll(validators.Select(v=> v.ValidateAsync(context,cancellationToken)));

    var failures = result.SelectMany(e=>e.Errors.Where(e=>e is not null)).ToList();
   if (failures.Count != 0)
   {
     throw new ValidationException(failures);
   }

   return await next(cancellationToken);
  }
 }