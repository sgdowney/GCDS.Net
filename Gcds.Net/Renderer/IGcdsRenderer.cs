using Gcds.Net.Models;

using Microsoft.AspNetCore.Html;

namespace Gcds.Net.Renderer
{
    public enum PageLanguage
    {
        en = 0,
        fr = 1
    }

    public interface IGcdsRenderer
    {
        public PageLanguage Language { get; set; }
        public IHtmlContent RenderHeader(IHeaderModel model);
        public IHtmlContent RenderSearch();
        public IHtmlContent RenderBreadcrumbs(IHeaderModel model);
        public IHtmlContent RenderBreadcrumbItem(IBreadcrumbItemModel model);
        public IHtmlContent RenderFooter(IFooterModel model);
        //public IHtmlContent RenderErrorSummary(ModelStateDictionary modelState, string? heading);
    }
}