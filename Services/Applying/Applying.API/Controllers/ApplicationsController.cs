using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.BuildingBlocks.EventBus.Extensions;
using Microsoft.Fee.Services.Applying.API.Application.Commands;
using Microsoft.Fee.Services.Applying.API.Application.Queries;
using Microsoft.Fee.Services.Applying.API.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Applying.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IApplicationQueries _applicationQueries;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(
            IMediator mediator,
            IApplicationQueries applicationQueries,
            IIdentityService identityService,
            ILogger<ApplicationsController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _applicationQueries = applicationQueries ?? throw new ArgumentNullException(nameof(applicationQueries));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("cancel")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelApplicationAsync([FromBody] CancelApplicationCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCancelApplication = new IdentifiedCommand<CancelApplicationCommand, bool>(command, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestCancelApplication.GetGenericTypeName(),
                    nameof(requestCancelApplication.Command.ApplicationNumber),
                    requestCancelApplication.Command.ApplicationNumber,
                    requestCancelApplication);

                commandResult = await _mediator.Send(requestCancelApplication);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("grant")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GrantApplicationAsync([FromBody] GrantApplicationCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestGrantApplication = new IdentifiedCommand<GrantApplicationCommand, bool>(command, guid);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestGrantApplication.GetGenericTypeName(),
                    nameof(requestGrantApplication.Command.ApplicationNumber),
                    requestGrantApplication.Command.ApplicationNumber,
                    requestGrantApplication);

                commandResult = await _mediator.Send(requestGrantApplication);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("{applicationId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Application.Queries.Application), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetApplicationAsync(int applicationId)
        {
            try
            {
                var application = await _applicationQueries.GetApplicationAsync(applicationId);

                return Ok(application);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationSummary>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ApplicationSummary>>> GetApplicationsAsync()
        {
            var userid = _identityService.GetUserIdentity();
            var applications = await _applicationQueries.GetApplicationsFromUserAsync(Guid.Parse(userid));

            return Ok(applications);
        }

        [Route("paymentTypes")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PaymentType>>> GetCardTypesAsync()
        {
            var paymentTypes = await _applicationQueries.GetPaymentTypesAsync();

            return Ok(paymentTypes);
        }

        [Route("draft")]
        [HttpPost]
        public async Task<ActionResult<ApplicationDraftDTO>> CreateApplicationDraftFromBasketDataAsync([FromBody] CreateApplicationDraftCommand createApplicationDraftCommand)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createApplicationDraftCommand.GetGenericTypeName(),
                nameof(createApplicationDraftCommand.StudentId),
                createApplicationDraftCommand.StudentId,
                createApplicationDraftCommand);

            return await _mediator.Send(createApplicationDraftCommand);
        }
    }
}
