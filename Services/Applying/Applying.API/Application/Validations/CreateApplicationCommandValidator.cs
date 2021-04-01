using FluentValidation;
using Microsoft.Fee.Services.Applying.API.Application.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.Fee.Services.Applying.API.Application.Commands.CreateApplicationCommand;

namespace Applying.API.Application.Validations
{
    public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
    {
        public CreateApplicationCommandValidator(ILogger<CreateApplicationCommandValidator> logger)
        {
            RuleFor(command => command.IDNumber).NotEmpty();
            RuleFor(command => command.Request).NotEmpty();
            RuleFor(command => command.PaymentTypeId).NotEmpty();
            RuleFor(command => command.ApplicationItems).Must(ContainApplicationItems).WithMessage("No application items found");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }

        private bool ContainApplicationItems(IEnumerable<ApplicationItemDTO> applicationItems)
        {
            return applicationItems.Any();
        }
    }
}