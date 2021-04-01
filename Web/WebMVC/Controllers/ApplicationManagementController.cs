using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.WebMVC.Services;
using Microsoft.Fee.WebMVC.ViewModels;
using System.Threading.Tasks;
using WebMVC.Services.ModelDTOs;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class ApplicationManagementController : Controller
    {
        private IApplyingService _applicationSvc;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;
        public ApplicationManagementController(IApplyingService applicationSvc, IIdentityParser<ApplicationUser> appUserParser)
        {
            _appUserParser = appUserParser;
            _applicationSvc = applicationSvc;
        }

        public async Task<IActionResult> Index()
        {
            var user = _appUserParser.Parse(HttpContext.User);
            var vm = await _applicationSvc.GetMyApplications(user);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ApplicationProcess(string applicationId, string actionCode)
        {
            if (ApplicationProcessAction.Grant.Code == actionCode)
            {
                await _applicationSvc.GrantApplication(applicationId);
            }

            return RedirectToAction("Index");
        }
    }
}
