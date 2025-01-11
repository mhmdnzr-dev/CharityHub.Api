using FluentValidation;

using MediatR;

namespace CharityHub.Core.Contract.Primitives.Validations;


public sealed class FluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public FluentValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }



    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only run validation if a validator is available for the request
        if (_validator != null)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        return await next();
    }
}
