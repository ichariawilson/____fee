﻿namespace Microsoft.Fee.Services.Scholarship.API.ViewModel
{
    public class EditImageViewModel : UploadImageViewModel
    {
        public int Id { get; set; }
        public string ExistingImage { get; set; }
    }
}
