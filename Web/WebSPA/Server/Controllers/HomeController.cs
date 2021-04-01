using Fee.WebSPA;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fee.WebSPA.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public HomeController(IWebHostEnvironment env, IOptionsSnapshot<AppSettings> settings)
        {
            _env = env;
            _settings = settings;
        }
        public IActionResult Configuration()
        {
            return Json(_settings.Value);
        }
    }
}