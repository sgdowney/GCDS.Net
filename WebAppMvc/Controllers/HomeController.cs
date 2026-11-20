using Gcds.Net.ActionFilters;
using Gcds.Net.Models;
using Gcds.Net.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using System.Diagnostics;

using WebAppMvc.Models;

namespace WebAppMvc.Controllers
{
    [GcDesignModelActionFilter]
    [AutoValidateAntiforgeryToken]
    public class HomeController(IGcDesignModelAccessor accessor, IStringLocalizer<SharedResource> sharedLocalizer, IStringLocalizer<HomeController> localizer) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ConfigureIndexPage();
            return View();
        }


        private void ConfigureIndexPage()
        {
            IGcDesignModel model = accessor.GcModel;
            model.Header.PageTitle = sharedLocalizer["ApplicationTitle"];
            model.Header.LanguageHref = $"?culture={sharedLocalizer["LanguageSwitch"]}";
            model.Header.ShowSearch = true;
            model.Header.ShowBreadcrumbs = true;
            model.Header.SkipToHref = "#main-content";
            model.Footer.Display = FooterDisplay.compact;
            model.Footer.SubLinks.Add("Terms and conditions", "https://www.canada.ca/en/transparency/terms.html");
            model.Footer.SubLinks.Add("Privacy", "https://www.canada.ca/en/transparency/privacy.html");
        }

        [HttpGet]
        public IActionResult DefaultHeaderFooter()
        {
            IGcDesignModel model = accessor.GcModel;
            model.Header.PageTitle = sharedLocalizer["ApplicationTitle"];
            model.Header.LanguageHref = $"?culture={sharedLocalizer["LanguageSwitch"]}";
            model.Header.ShowSearch = true;
            model.Header.ShowBreadcrumbs = true;
            model.Header.Breadcrumbs.Add(new BreadcrumbItemModel { LinkText = localizer["BreadcrumbHome"], Href = "/" });
            return View();
        }

        [HttpGet]
        public IActionResult CompactFooter()
        {
            //IGcDesignModel model = accessor.GcModel;
            //model.Header.PageTitle = sharedLocalizer["ApplicationTitle"];
            //model.Header.LanguageHref = $"?culture={sharedLocalizer["LanguageSwitch"]}";
            //model.Header.ShowSearch = true;
            //model.Header.ShowBreadcrumbs = true;
            //model.Header.SkipToHref = "#main-content";
            //model.Header.Breadcrumbs.Add(new BreadcrumbItemModel { LinkText = localizer["BreadcrumbHome"], Href = "/" });
            //model.Footer.Display = FooterDisplay.compact;
            //model.Footer.SubLinks.Add("Terms and conditions", "https://www.canada.ca/en/transparency/terms.html");
            //model.Footer.SubLinks.Add("Privacy", "https://www.canada.ca/en/transparency/privacy.html");
            ConfigureCompactFooter();
            return View();
        }

        private void ConfigureCompactFooter()
        {
            IGcDesignModel model = accessor.GcModel;
            model.Header.PageTitle = sharedLocalizer["ApplicationTitle"];
            model.Header.LanguageHref = $"?culture={sharedLocalizer["LanguageSwitch"]}";
            model.Header.ShowSearch = true;
            model.Header.ShowBreadcrumbs = true;
            model.Header.SkipToHref = "#main-content";
            model.Header.Breadcrumbs.Add(new BreadcrumbItemModel { LinkText = localizer["BreadcrumbHome"], Href = "/" });
            model.Footer.Display = FooterDisplay.compact;
            model.Footer.SubLinks.Add("Terms and conditions", "https://www.canada.ca/en/transparency/terms.html");
            model.Footer.SubLinks.Add("Privacy", "https://www.canada.ca/en/transparency/privacy.html");
        }


        [HttpGet]
        public IActionResult GcdsInputDemo()
        {
            ConfigureCompactFooter();
            GcdsInputViewModel model = new() { PrefilledField = "Some prefilled text." };
            return View(model);
        }

        [HttpPost]
        public IActionResult GcdsInputDemo(GcdsInputViewModel model)
        {
            ConfigureCompactFooter();
            if (ModelState.IsValid)
            {
                return View("GcdsInputDemoValid", model);
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult GcdsRadiosDemo()
        {
            ConfigureCompactFooter();
            GcdsRadiosViewModel model = new() { FavouriteSeasonPreselected = "Summer", LeastFavouriteSeasonPreselected = "Winter" };
            return View(model);
        }

        [HttpPost]
        public IActionResult GcdsRadiosDemo(GcdsRadiosViewModel model)
        {
            ConfigureCompactFooter();
            if (ModelState.IsValid)
            {
                return View("GcdsRadiosDemoValid", model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GcdsCheckboxesDemo()
        {
            ConfigureCompactFooter();
            GcdsCheckboxesViewModel model = new() { FavouriteSeasonsPreselected = ["Summer"], LeastFavouriteSeasonsPreselected = ["Winter", "Fall"] };
            return View(model);
        }

        [HttpPost]
        public IActionResult GcdsCheckboxesDemo(GcdsCheckboxesViewModel model)
        {
            ConfigureCompactFooter();
            if (ModelState.IsValid)
            {
                return View("GcdsCheckboxesDemoValid", model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GcdsSelectDemo()
        {
            ConfigureCompactFooter();
            GcdsSelectViewModel model = new() { ProvinceOfWork = "3" };
            return View(model);
        }

        [HttpPost]
        public IActionResult GcdsSelectDemo(GcdsSelectViewModel model)
        {
            ConfigureCompactFooter();
            if (ModelState.IsValid)
            {
                return View("GcdsSelectDemoValid", model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GcdsDateInputDemo()
        {
            ConfigureCompactFooter();
            GcdsDateInputViewModel model = new() { FullDateIsoValuePreFilled = DateTime.Today.ToString("yyyy-MM-dd"), FullDateValuePreFilled = DateTime.Today.ToString("yyyy-MM-dd"), YearMonthValuePreFilled = DateTime.Today.ToString("yyyy-MM") };
            return View(model);
        }

        [HttpPost]
        public IActionResult GcdsDateInputDemo(GcdsDateInputViewModel model)
        {
            ConfigureCompactFooter();
            if (ModelState.IsValid)
            {
                return View("GcdsDateInputDemoValid", model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GcdsTextareaDemo()
        {
            ConfigureCompactFooter();
            GcdsTextareaViewModel model = new() { Comments = "This is a comment that was already entered. This is a comment that was already entered. This is a comment that was already entered. This is a comment that was already entered." };
            return View(model);
        }

        [HttpPost]
        public IActionResult GcdsTextareaDemo(GcdsTextareaViewModel model)
        {
            ConfigureCompactFooter();
            if (ModelState.IsValid)
            {
                return View("GcdsTextareaDemoValid", model);
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
