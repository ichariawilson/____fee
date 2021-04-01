using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Fee.Mobile.Scholarship.HttpAggregator.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet()]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
