using ActionServiceAPI.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace ActionServiceAPI.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var errors = validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (errors.Count != 0)
                throw new ActionDomainException($"Validation of {typeof(TRequest).Name} failed!", new ValidationException("Failures:", errors));

            return await next();
        }
    }
}
