using Gcds.Net.Models;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Globalization;
using System.Text.Json;

namespace Gcds.Net.Renderer
{
    public class GcdsRenderer : IGcdsRenderer
    {
        public GcdsRenderer()
        {
            Language = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName.Equals("fra", StringComparison.OrdinalIgnoreCase)
                ? PageLanguage.fr
                : PageLanguage.en;
        }
        public PageLanguage Language { get; set; }

        public IHtmlContent RenderHeader(IHeaderModel model)
        {
            TagBuilder slotBuilder;
            TagBuilder headerTagBuilder = new("gcds-header");
            SetLanguage(headerTagBuilder);
            headerTagBuilder.Attributes.Add("lang-href", model.LanguageHref);
            headerTagBuilder.Attributes.Add("skip-to-href", model.SkipToHref);

            if (model.ShowSearch)
            {
                slotBuilder = new TagBuilder("div");
                slotBuilder.Attributes.Add("slot", "search");
                slotBuilder.InnerHtml.AppendHtml(RenderSearch());
                headerTagBuilder.InnerHtml.AppendHtml(slotBuilder);
            }
            if (model.ShowBreadcrumbs)
            {
                slotBuilder = new TagBuilder("div");
                slotBuilder.Attributes.Add("slot", "breadcrumb");
                slotBuilder.InnerHtml.AppendHtml(RenderBreadcrumbs(model));
                headerTagBuilder.InnerHtml.AppendHtml(slotBuilder);
            }
            return headerTagBuilder;
        }

        public IHtmlContent RenderSearch()
        {
            TagBuilder searchTagBuilder = new("gcds-search");
            SetLanguage(searchTagBuilder);
            return searchTagBuilder;
        }

        public IHtmlContent RenderBreadcrumbs(IHeaderModel model)
        {
            TagBuilder breadcrumbsTagBuilder = new("gcds-breadcrumbs");
            SetLanguage(breadcrumbsTagBuilder);
            foreach (var item in model.Breadcrumbs)
            {
                breadcrumbsTagBuilder.InnerHtml.AppendHtml(RenderBreadcrumbItem(item));
            }
            return breadcrumbsTagBuilder;
        }

        public IHtmlContent RenderBreadcrumbItem(IBreadcrumbItemModel model)
        {
            TagBuilder breadcrumbItemTagBuilder = new("gcds-breadcrumbs-item");
            SetLanguage(breadcrumbItemTagBuilder);
            breadcrumbItemTagBuilder.Attributes.Add("href", model.Href);
            breadcrumbItemTagBuilder.InnerHtml.Append(model.LinkText!);
            return breadcrumbItemTagBuilder;
        }

        private void SetLanguage(TagBuilder tagBuilder)
        {
            tagBuilder.Attributes.Add("lang", Language.ToString());
        }

        public IHtmlContent RenderFooter(IFooterModel model)
        {
            TagBuilder footerTagBuilder = new("gcds-footer");
            SetLanguage(footerTagBuilder);
            footerTagBuilder.Attributes.Add("display", model.Display.ToString());
            if (!string.IsNullOrWhiteSpace(model.ContextualHeading))
            {
                footerTagBuilder.Attributes.Add("contextual-heading", model.ContextualHeading);
                footerTagBuilder.Attributes.Add("contextual-links", JsonSerializer.Serialize(model.ContextualLinks));
            }
            if (model.SubLinks.Count > 0)
            {
                footerTagBuilder.Attributes.Add("sub-links", JsonSerializer.Serialize(model.SubLinks));
            }
            return footerTagBuilder;
        }


        //public IHtmlContent RenderErrorSummary(ModelStateDictionary modelState, string? heading)
        //{
        //    TagBuilder summaryTag = new("gcds-error-summary");
        //    SetLanguage(summaryTag);
        //    if (!string.IsNullOrWhiteSpace(heading))
        //    {
        //        summaryTag.Attributes.Add("heading", heading);
        //    }
        //    if (!modelState.IsValid)
        //    {
        //        Dictionary<string, string> errorLinks = [];
        //        foreach (var item in modelState)
        //        {
        //            if (item.Value.ValidationState == ModelValidationState.Invalid)
        //            {
        //                errorLinks.Add($"#gcds-{item.Key}", item.Value.Errors[0].ErrorMessage);
        //            }
        //        }
        //        summaryTag.Attributes.Add("error-links", JsonSerializer.Serialize(errorLinks));
        //    }
        //    return summaryTag;
        //}
    }
}
