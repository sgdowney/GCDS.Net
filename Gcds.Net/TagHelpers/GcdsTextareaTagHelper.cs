using Gcds.Net.Models;
using Gcds.Net.Properties;
using Gcds.Net.Renderer;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gcds.Net.TagHelpers
{
    [HtmlTargetElement("gcds-textarea", Attributes = ForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GcdsTextareaTagHelper(IGcdsHtmlGenerator generator) : TagHelper
    {
        private const string ForAttributeName = "gcds-for";

        public override int Order => -1000;

        /// <summary>
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="InputTagHelper"/>'s output.
        /// </summary>
        protected IGcdsHtmlGenerator Generator { get; } = generator;

        [HtmlAttributeNotBound]
        [ViewContext]
        public required ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public required ModelExpression For { get; set; }

        public string? Name { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);

            if (Name != null)
            {
                output.CopyHtmlAttribute("name", context);
            }

            // Note null or empty For.Name is allowed because TemplateInfo.HtmlFieldPrefix may be sufficient.
            // IHtmlGenerator will enforce name requirements.
            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;
            if (metadata == null)
            {
                throw new InvalidOperationException(ResourceFormat.FormatTagHelpers_NoProvidedMetadata(
                "<gcds-textarea>",
                ForAttributeName,
                nameof(IModelMetadataProvider),
                For.Name));
            }

            // Ensure Generator does not throw due to empty "fullName" if user provided a name attribute.
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); //null;
            if (string.IsNullOrEmpty(For.Name) &&
                string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
                !string.IsNullOrEmpty(Name))
            {
                htmlAttributes["name"] = Name;
            }
            else
            {
                htmlAttributes.Add("name", For.Name);
            }
            TagBuilder tagBuilder;
            tagBuilder = Generator.GenerateTextArea(ViewContext, modelExplorer, For.Name, Rows, Cols, htmlAttributes);
            if (tagBuilder != null)
            {
                //if (tagBuilder.Attributes.TryGetValue("required", out _))
                //{
                //    output.Attributes.Add(new TagHelperAttribute("required", value: null, valueStyle: HtmlAttributeValueStyle.Minimized));
                //}

                // This TagBuilder contains the primary <input/> element of interest.
                output.MergeAttributes(tagBuilder);


                if (tagBuilder.HasInnerHtml)
                {
                    // Since this is not the "checkbox" special-case, no guarantee that output is a self-closing
                    // element. A later tag helper targeting this element may change output.TagMode.
                    output.Content.AppendHtml(tagBuilder.InnerHtml);
                }
            }
            string inputName = output.Attributes["name"].Value.ToString()!;
            output.Attributes.Add("id", $"gcds-{inputName}");
            if (!ViewContext.ModelState.IsValid)
            {
                if (ViewContext.ModelState.TryGetValue(inputName, out ModelStateEntry? entry))
                {
                    if (entry.ValidationState == ModelValidationState.Invalid)
                    {
                        output.Attributes.Add("error-message", entry.Errors[0].ErrorMessage);
                    }
                }
            }
        }

    }
}
