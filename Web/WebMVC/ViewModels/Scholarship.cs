using System.Collections.Generic;

namespace Microsoft.Fee.WebMVC.ViewModels
{
    public record Scholarship
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public int Count { get; init; }
        public List<ScholarshipItem> Data { get; init; }
    }
}
