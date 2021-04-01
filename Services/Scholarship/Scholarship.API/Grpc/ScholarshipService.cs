using ScholarshipApi;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Scholarship.API;
using Microsoft.Fee.Services.Scholarship.API.Infrastructure;
using Microsoft.Fee.Services.Scholarship.API.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ScholarshipApi.Scholarship;

namespace Scholarship.API.Grpc
{
    public class ScholarshipService : ScholarshipBase
    {
        private readonly ScholarshipContext _scholarshipContext;
        private readonly ScholarshipSettings _settings;
        private readonly ILogger _logger;

        public ScholarshipService(ScholarshipContext dbContext, IOptions<ScholarshipSettings> settings, ILogger<ScholarshipService> logger)
        {
            _settings = settings.Value;
            _scholarshipContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        public override async Task<ScholarshipItemResponse> GetItemById(ScholarshipItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ScholarshipService.GetItemById for product id {Id}", request.Id);
            if (request.Id <= 0)
            {
                context.Status = new Status(StatusCode.FailedPrecondition, $"Id must be > 0 (received {request.Id})");
                return null;
            }

            var item = await _scholarshipContext.ScholarshipItems.SingleOrDefaultAsync(ci => ci.Id == request.Id);
            var baseUri = _settings.PicBaseUrl;
            var azureStorageEnabled = _settings.AzureStorageEnabled;
            item.FillScholarshipItemUrl(baseUri, azureStorageEnabled: azureStorageEnabled);

            if (item != null)
            {
                return new ScholarshipItemResponse()
                {
                    AvailableSlots = item.AvailableSlots,
                    Description = item.Description,
                    Id = item.Id,
                    MaxSlotsThreshold = item.MaxSlotThreshold,
                    Name = item.Name,
                    OnReapply = item.OnReapply,
                    PictureFileName = item.PictureFileName,
                    PictureUri = item.PictureUri,
                    Amount = (double)item.Amount,
                    ReslotThreshold = item.ReslotThreshold
                };
            }

            context.Status = new Status(StatusCode.NotFound, $"ScholarshipItem with id {request.Id} do not exist");
            return null;
        }

        public override async Task<PaginatedItemsResponse> GetItemsByIds(ScholarshipItemsRequest request, ServerCallContext context)
        {
            if (!string.IsNullOrEmpty(request.Ids))
            {
                var items = await GetItemsByIdsAsync(request.Ids);

                context.Status = !items.Any() ?
                    new Status(StatusCode.NotFound, $"ids value invalid. Must be comma-separated list of numbers") :
                    new Status(StatusCode.OK, string.Empty);

                return this.MapToResponse(items);
            }

            var totalItems = await _scholarshipContext.ScholarshipItems
                .LongCountAsync();

            var itemsOnPage = await _scholarshipContext.ScholarshipItems
                .OrderBy(c => c.Name)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = this.MapToResponse(itemsOnPage, totalItems, request.PageIndex, request.PageSize);
            context.Status = new Status(StatusCode.OK, string.Empty);

            return model;
        }

        private PaginatedItemsResponse MapToResponse(List<ScholarshipItem> items)
        {
            return this.MapToResponse(items, items.Count, 1, items.Count);
        }

        private PaginatedItemsResponse MapToResponse(List<ScholarshipItem> items, long count, int pageIndex, int pageSize)
        {
            var result = new PaginatedItemsResponse()
            {
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            items.ForEach(i =>
            {
                var scholarshipcurrency = i.ScholarshipCurrency == null
                                            ? null
                                            : new ScholarshipApi.ScholarshipCurrency()
                                            {
                                                Id = i.ScholarshipCurrency.Id,
                                                Symbol = i.ScholarshipCurrency.Symbol,
                                                Code = i.ScholarshipCurrency.Code,
                                                Currency = i.ScholarshipCurrency.Currency,
                                            };
                var scholarshipduration = i.ScholarshipDuration == null
                                            ? null
                                            : new ScholarshipApi.ScholarshipDuration()
                                            {
                                                Id = i.ScholarshipDuration.Id,
                                                Duration = i.ScholarshipDuration.Duration,
                                            };
                var scholarshipeducationLevel = i.ScholarshipEducationLevel == null
                                            ? null
                                            : new ScholarshipApi.ScholarshipEducationLevel()
                                            {
                                                Id = i.ScholarshipEducationLevel.Id,
                                                EducationLevel = i.ScholarshipEducationLevel.EducationLevel,
                                            };
                var scholarshipinterest = i.ScholarshipInterest == null
                                            ? null
                                            : new ScholarshipApi.ScholarshipInterest()
                                            {
                                                Id = i.ScholarshipInterest.Id,
                                                Interest = i.ScholarshipInterest.Interest,
                                            };
                var scholarshiplocation = i.ScholarshipLocation == null
                                            ? null
                                            : new ScholarshipApi.ScholarshipLocation()
                                            {
                                                Id = i.ScholarshipLocation.Id,
                                                Location = i.ScholarshipLocation.Location,
                                            };

                result.Data.Add(new ScholarshipItemResponse()
                {
                    AvailableSlots = i.AvailableSlots,
                    Description = i.Description,
                    Id = i.Id,
                    MaxSlotsThreshold = i.MaxSlotThreshold,
                    Name = i.Name,
                    OnReapply = i.OnReapply,
                    PictureFileName = i.PictureFileName,
                    PictureUri = i.PictureUri,
                    ReslotThreshold = i.ReslotThreshold,
                    ScholarshipCurrency = scholarshipcurrency,
                    ScholarshipDuration = scholarshipduration,
                    ScholarshipEducationLevel = scholarshipeducationLevel,
                    ScholarshipInterest = scholarshipinterest,
                    ScholarshipLocation = scholarshiplocation,
                    Amount = (double)i.Amount,
                });
            });

            return result;
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