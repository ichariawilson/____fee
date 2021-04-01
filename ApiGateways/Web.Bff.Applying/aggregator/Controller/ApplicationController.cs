using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.Web.Applying.HttpAggregator.Models;
using Microsoft.Fee.Web.Applying.HttpAggregator.Services;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IApplyingService _applyingService;
        public ApplicationController(IBasketService basketService, IApplyingService applyingService)
        {
            _basketService = basketService;
            _applyingService = applyingService;
        }

        [Route("draft/{basketId}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationData), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApplicationData>> GetApplicationDraftAsync(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                return BadRequest("Need a valid basketid");
            }
            // Get the basket data and build a application draft based on it
            var basket = await _basketService.GetById(basketId);

            if (basket == null)
            {
                return BadRequest($"No basket found for id {basketId}");
            }

            return await _applyingService.GetApplicationDraftAsync(basket);
        }
    }
}
