using Gcds.Net.Renderer;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System.Text.Json;

namespace Gcds.Net.TagHelpers
{
    [HtmlTargetElement("gcds-error-summary", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GcdsErrorSummaryTagHelper() : TagHelper
    {
        public override int Order => base.Order;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        [HtmlAttributeName("heading")]
        public string? Heading { get; set; }

        [HtmlAttributeName("lang")]
        public PageLanguage? Language { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Heading != null)
            {
                output.CopyHtmlAttribute("heading", context);
            }
            if (Language != null)
            {
                output.CopyHtmlAttribute("lang", context);
            }
            TagBuilder summaryTag = new("gcds-error-summary");
            if (!ViewContext!.ModelState.IsValid)
            {
                Dictionary<string, string> errorLinks = [];
                foreach (var item in ViewContext!.ModelState)
                {
                    if (item.Value.ValidationState == ModelValidationState.Invalid)
                    {
                        errorLinks.Add($"#gcds-{item.Key}", item.Value.Errors[0].ErrorMessage);
                    }
                }
                summaryTag.Attributes.Add("error-links", JsonSerializer.Serialize(errorLinks));
            }
            output.MergeAttributes(summaryTag);
            // Do not update the content if another tag helper targeting this element has already done so.
            if (!output.IsContentModified && summaryTag.HasInnerHtml)
            {
                output.Content.SetHtmlContent(summaryTag.InnerHtml);
            }
            //TagBuilder tagBuilder = (TagBuilder)gcdsRenderer.RenderErrorSummary(ViewContext!.ModelState, Heading);
            //if (tagBuilder != null)
            //{
            //    output.MergeAttributes(tagBuilder);
            //    // Do not update the content if another tag helper targeting this element has already done so.
            //    if (!output.IsContentModified && tagBuilder.HasInnerHtml)
            //    {
            //        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            //    }
            //}
        }

        //public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    return Task.Run(() => Process(context, output));
        //    //return base.ProcessAsync(context, output);
        //}
    }
}
