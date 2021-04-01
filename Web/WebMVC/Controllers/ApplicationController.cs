using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.WebMVC.Services;
using Microsoft.Fee.WebMVC.ViewModels;
using System;
using System.Threading.Tasks;

namespace Microsoft.Fee.WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class ApplicationController : Controller
    {
        private IApplyingService _applicationSvc;
        private IBasketService _basketSvc;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;
        public ApplicationController(IApplyingService applicationSvc, IBasketService basketSvc, IIdentityParser<ApplicationUser> appUserParser)
        {
            _appUserParser = appUserParser;
            _applicationSvc = applicationSvc;
            _basketSvc = basketSvc;
        }

        public async Task<IActionResult> Create()
        {

            var user = _appUserParser.Parse(HttpContext.User);
            var application = await _basketSvc.GetApplicationDraft(user.Id);
            var vm = _applicationSvc.MapUserInfoIntoApplication(user, application);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Application model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _appUserParser.Parse(HttpContext.User);
                    var basket = _applicationSvc.MapApplicationToBasket(model);

                    await _basketSvc.Checkout(basket);

                    //Redirect to historic list.
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", $"It was not possible to create a new application, please try later on ({ex.GetType().Name} - {ex.Message})");
            }

            return View("Create", model);
        }

        public async Task<IActionResult> Cancel(string applicationId)
        {
            await _applicationSvc.CancelApplication(applicationId);

            //Redirect to historic list.
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(string applicationId)
        {
            var user = _appUserParser.Parse(HttpContext.User);

            var application = await _applicationSvc.GetApplication(user, applicationId);
            return View(application);
        }

        public async Task<IActionResult> Index(Application item)
        {
            var user = _appUserParser.Parse(HttpContext.User);
            var vm = await _applicationSvc.GetMyApplications(user);
            return View(vm);
        }
    }
}