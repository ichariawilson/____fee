namespace Microsoft.Fee.Services.Scholarship.API.Model
{
    public static class ScholarshipItemExtensions
    {
        public static void FillScholarshipItemUrl(this ScholarshipItem item, string picBaseUrl, bool azureStorageEnabled)
        {
            if (item != null)
            {
                item.PictureUri = azureStorageEnabled
                   ? picBaseUrl + item.PictureFileName
                   : picBaseUrl.Replace("[0]", item.Id.ToString());
            }
        }
    }
}
