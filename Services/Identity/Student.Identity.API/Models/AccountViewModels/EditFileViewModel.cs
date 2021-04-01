namespace Microsoft.Fee.Services.Student.Identity.API.Models.AccountViewModels
{
    public class EditFileViewModel : UploadFileViewModel
    {
        public int Id { get; set; }
        public string ExistingFile { get; set; }
    }
}
