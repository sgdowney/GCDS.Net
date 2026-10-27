using Gcds.Net.ActionFilters;
using Gcds.Net.Models;
using Gcds.Net.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System.Diagnostics;

using WebAppMvc.Models;
using WebAppMvc.Resources;

namespace WebAppMvc.Controllers
{
    [GcDesignModelActionFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGcDesignModelAccessor _accessor;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public HomeController(ILogger<HomeController> logger, IGcDesignModelAccessor accessor, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _logger = logger;
            _accessor = accessor;
            _sharedLocalizer = sharedLocalizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            LoginViewModel viewModel = new() { HiddenField = "Hide me!", AnotherHiddenField = "Hide me too!" };
            ConfigureIndexPage();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel viewModel)
        {
            ConfigureIndexPage();
            if (ModelState.IsValid)
            {
                return View("IndexValid", viewModel);
            }
            return View(viewModel);
        }

        private void ConfigureIndexPage()
        {
            IGcDesignModel model = _accessor.GcModel;

            model.Footer.SubLinks.Add("Terms and conditions", "https://www.canada.ca/en/transparency/terms.html");
            model.Footer.SubLinks.Add("Privacy", "https://www.canada.ca/en/transparency/privacy.html");
            model.Header.PageTitle = _sharedLocalizer["ApplicationTitle"];
            model.Header.LanguageHref = $"?culture={_sharedLocalizer["LanguageSwitch"]}";
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
