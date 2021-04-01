using Scholarship.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Scholarship.API.Infrastructure;
using Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events;
using Microsoft.Fee.Services.Scholarship.API.Model;
using Microsoft.Fee.Services.Scholarship.API.ViewModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Fee.Services.Scholarship.API.Services;

namespace Microsoft.Fee.Services.Scholarship.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ScholarshipController : ControllerBase
    {
        private readonly ScholarshipContext _scholarshipContext;
        private readonly ScholarshipSettings _settings;
        private readonly IIdentityService _identityService;
        private readonly IScholarshipIntegrationEventService _scholarshipIntegrationEventService;

        public ScholarshipController(ScholarshipContext context, IOptionsSnapshot<ScholarshipSettings> settings, IIdentityService identityService, IScholarshipIntegrationEventService scholarshipIntegrationEventService)
        {
            _scholarshipContext = context ?? throw new ArgumentNullException(nameof(context));
            _scholarshipIntegrationEventService = scholarshipIntegrationEventService ?? throw new ArgumentNullException(nameof(scholarshipIntegrationEventService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ScholarshipItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ScholarshipItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIdsAsync(ids);

                if (!items.Any())
                {
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");
                }

                return Ok(items);
            }

            var totalItems = await _scholarshipContext.ScholarshipItems
                .LongCountAsync();

            var itemsOnPage = await _scholarshipContext.ScholarshipItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<ScholarshipItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        private async Task<List<ScholarshipItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<ScholarshipItem>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _scholarshipContext.ScholarshipItems.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            items = ChangeUriPlaceholder(items);

            return items;
        }

        // GET api/v1/[controller]/items/{id:int}
        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ScholarshipItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ScholarshipItem>> ItemByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _scholarshipContext.ScholarshipItems.SingleOrDefaultAsync(si => si.Id == id);

            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;

            item.FillScholarshipItemUrl(baseUri, azureStorageEnabled: azureStorageEnabled);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        // GET api/v1/[controller]/items/withname/samplename[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ScholarshipItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ScholarshipItem>>> ItemsWithNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _scholarshipContext.ScholarshipItems
                .Where(c => c.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _scholarshipContext.ScholarshipItems
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<ScholarshipItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/educationlevel/1/location[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/educationlevel/{scholarshipEducationLevelId}/location/{scholarshipLocationId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ScholarshipItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ScholarshipItem>>> ItemsByEducationLevelIdAndLocationIdAsync(int scholarshipEducationLevelId, int? scholarshipLocationId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<ScholarshipItem>)_scholarshipContext.ScholarshipItems;

            root = root.Where(si => si.ScholarshipEducationLevelId == scholarshipEducationLevelId);

            if (scholarshipLocationId.HasValue)
            {
                root = root.Where(si => si.ScholarshipLocationId == scholarshipLocationId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<ScholarshipItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/items/educationlevel/all/location[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items/educationlevel/all/location/{scholarshipLocationId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ScholarshipItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ScholarshipItem>>> ItemsByLocationIdAsync(int? scholarshipLocationId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<ScholarshipItem>)_scholarshipContext.ScholarshipItems;

            if (scholarshipLocationId.HasValue)
            {
                root = root.Where(si => si.ScholarshipLocationId == scholarshipLocationId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<ScholarshipItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        // GET api/v1/[controller]/ScholarshipCurrencies
        [HttpGet]
        [Route("scholarshipcurrencies")]
        [ProducesResponseType(typeof(List<ScholarshipCurrency>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ScholarshipCurrency>>> ScholarshipCurrenciesAsync()
        {
            return await _scholarshipContext.ScholarshipCurrencies.ToListAsync();
        }

        // GET api/v1/[controller]/ScholarshipDurations
        [HttpGet]
        [Route("scholarshipdurations")]
        [ProducesResponseType(typeof(List<ScholarshipDuration>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ScholarshipDuration>>> ScholarshipDurationsAsync()
        {
            return await _scholarshipContext.ScholarshipDurations.ToListAsync();
        }

        // GET api/v1/[controller]/EducationLevels
        [HttpGet]
        [Route("scholarshipeducationlevels")]
        [ProducesResponseType(typeof(List<ScholarshipEducationLevel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ScholarshipEducationLevel>>> ScholarshipEducationLevelsAsync()
        {
            return await _scholarshipContext.ScholarshipEducationLevels.ToListAsync();
        }

        // GET api/v1/[controller]/ScholarshipFeeStuctures
        [HttpGet]
        [Route("scholarshipfeestructures")]
        [ProducesResponseType(typeof(List<ScholarshipFeeStructure>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ScholarshipFeeStructure>>> ScholarshipFeeStructuresAsync()
        {
            return await _scholarshipContext.ScholarshipFeeStructures.ToListAsync();
        }

        // GET api/v1/[controller]/ScholarshipInterests
        [HttpGet]
        [Route("scholarshipinterests")]
        [ProducesResponseType(typeof(List<ScholarshipInterest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ScholarshipInterest>>> ScholarshipInterestsAsync()
        {
            return await _scholarshipContext.ScholarshipInterests.ToListAsync();
        }

        // GET api/v1/[controller]/ScholarshipLocations
        [HttpGet]
        [Route("scholarshiplocations")]
        [ProducesResponseType(typeof(List<ScholarshipLocation>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ScholarshipLocation>>> ScholarshipLocationsAsync()
        {
            return await _scholarshipContext.ScholarshipLocations.ToListAsync();
        }

        //PUT api/v1/[controller]/items
        [Authorize]
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateScholarshipItemAsync([FromBody] ScholarshipItem scholarshipItemToUpdate)
        {
            var scholarshipItem = await _scholarshipContext.ScholarshipItems.SingleOrDefaultAsync(i => i.Id == scholarshipItemToUpdate.Id);

            if (scholarshipItem == null)
            {
                return NotFound(new { Message = $"Item with id {scholarshipItemToUpdate.Id} not found." });
            }

            var oldAmount = scholarshipItem.Amount;
            var raiseScholarshipItemAmountChangedEvent = oldAmount != scholarshipItemToUpdate.Amount;

            // Update current scholarshipItem
            scholarshipItem = scholarshipItemToUpdate;
            _scholarshipContext.ScholarshipItems.Update(scholarshipItem);

            if (raiseScholarshipItemAmountChangedEvent) // Save scholarshipItem's data and publish integration event through the Event Bus if Amount has changed
            {
                //Create Integration Event to be published through the Event Bus
                var amountChangedEvent = new ScholarshipItemAmountChangedIntegrationEvent(scholarshipItem.Id, scholarshipItemToUpdate.Amount, oldAmount);

                // Achieving atomicity between original Scholarship database operation and the IntegrationEventLog thanks to a local transaction
                await _scholarshipIntegrationEventService.SaveEventAndScholarshipContextChangesAsync(amountChangedEvent);

                // Publish through the Event Bus and mark the saved event as published
                await _scholarshipIntegrationEventService.PublishThroughEventBusAsync(amountChangedEvent);
            }
            else // Just save the updated scholarshipItem because the ScholarshipItem's Amount hasn't changed.
            {
                await _scholarshipContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = scholarshipItemToUpdate.Id }, null);
        }

        //POST api/v1/[controller]/items
        [Authorize]
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateScholarshipItemAsync([FromBody] ScholarshipItem scholarshipItem)
        {
            var item = new ScholarshipItem
            {
                ScholarshipCurrencyId = scholarshipItem.ScholarshipCurrencyId,
                ScholarshipDurationId = scholarshipItem.ScholarshipDurationId,
                ScholarshipEducationLevelId = scholarshipItem.ScholarshipEducationLevelId,
                ScholarshipInterestId = scholarshipItem.ScholarshipInterestId,
                ScholarshipLocationId = scholarshipItem.ScholarshipLocationId,
                Description = scholarshipItem.Description,
                Name = scholarshipItem.Name,
                PictureFileName = scholarshipItem.PictureFileName,
                Amount = scholarshipItem.Amount
            };

            _scholarshipContext.ScholarshipItems.Add(item);

            await _scholarshipContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Authorize]
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteScholarshipItemAsync(int id)
        {
            var scholarshipItem = _scholarshipContext.ScholarshipItems.SingleOrDefault(x => x.Id == id);

            if (scholarshipItem == null)
            {
                return NotFound();
            }

            _scholarshipContext.ScholarshipItems.Remove(scholarshipItem);

            await _scholarshipContext.SaveChangesAsync();

            return NoContent();
        }

        private List<ScholarshipItem> ChangeUriPlaceholder(List<ScholarshipItem> items)
        {
            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;

            foreach (var item in items)
            {
                item.FillScholarshipItemUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
            }
            return items;
        }
    }
}
