﻿namespace Microsoft.Fee.Services.Sponsor.API.Models.ViewModels
{
    public class EditImageViewModel : UploadImageViewModel
    {
        public int Id { get; set; }
        public string ExistingImage { get; set; }
    }
}