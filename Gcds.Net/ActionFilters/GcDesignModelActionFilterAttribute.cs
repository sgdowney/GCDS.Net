using Gcds.Net.Models;
using Gcds.Net.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gcds.Net.ActionFilters
{
    public class GcDesignModelActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                if (controller.HttpContext.RequestServices.GetService(typeof(IGcDesignModelAccessor)) is IGcDesignModelAccessor modelAccessor)
                {
                    IGcDesignModel model = modelAccessor.GcModel;
                    controller.ViewData[nameof(GcDesignModel)] = model;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
