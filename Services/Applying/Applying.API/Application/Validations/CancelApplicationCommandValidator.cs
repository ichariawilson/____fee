using FluentValidation;
using Microsoft.Extensions.Logging;
using Applying.API.Application.Commands;

namespace Applying.API.Application.Validations
{
    public class CancelApplicationCommandValidator : AbstractValidator<CancelApplicationCommand>
    {
        public CancelApplicationCommandValidator(ILogger<CancelApplicationCommandValidator> logger)
        {
            RuleFor(application => application.ApplicationNumber).NotEmpty().WithMessage("No applicationId found");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}