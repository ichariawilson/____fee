using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.WebMVC.Services;
using Microsoft.Fee.WebMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Fee.WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class CartController : Controller
    {
        private readonly IBasketService _basketSvc;
        private readonly IScholarshipService _catalogSvc;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;

        public CartController(IBasketService basketSvc, IScholarshipService catalogSvc, IIdentityParser<ApplicationUser> appUserParser)
        {
            _basketSvc = basketSvc;
            _catalogSvc = catalogSvc;
            _appUserParser = appUserParser;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var user = _appUserParser.Parse(HttpContext.User);
                var vm = await _basketSvc.GetBasket(user);

                return View(vm);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> slots, string action)
        {
            try
            {
                var user = _appUserParser.Parse(HttpContext.User);
                var basket = await _basketSvc.SetSlots(user, slots);
                if (action == "[ Checkout ]")
                {
                    return RedirectToAction("Create", "Application");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return View();
        }

        public async Task<IActionResult> AddToCart(ScholarshipItem scholarshipItemDetails)
        {
            try
            {
                if (scholarshipItemDetails?.Id != null)
                {
                    var user = _appUserParser.Parse(HttpContext.User);
                    await _basketSvc.AddItemToBasket(user, scholarshipItemDetails.Id);
                }
                return RedirectToAction("Index", "Scholarship");
            }
            catch (Exception ex)
            {
                // Catch error when Basket.api is in circuit-opened mode                 
                HandleException(ex);
            }

            return RedirectToAction("Index", "Scholarship", new { errorMsg = ViewBag.BasketInoperativeMsg });
        }

        private void HandleException(Exception ex)
        {
            ViewBag.BasketInoperativeMsg = $"Basket Service is inoperative {ex.GetType().Name} - {ex.Message}";
        }
    }
}
