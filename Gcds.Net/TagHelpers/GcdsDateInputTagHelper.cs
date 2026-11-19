using Gcds.Net.Properties;
using Gcds.Net.Renderer;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System.Reflection.Emit;

namespace Gcds.Net.TagHelpers
{
    [HtmlTargetElement("gcds-date-input", Attributes = ForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GcdsDateInputTagHelper(IGcdsHtmlGenerator generator) : TagHelper
    {
        private const string ForAttributeName = "gcds-for";
        private const string FormatAttributeName = "format";

        protected IGcdsHtmlGenerator Generator { get; } = generator;

        public override int Order => -1000;

        [HtmlAttributeNotBound]
        [ViewContext]
        public required ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public required ModelExpression For { get; set; }

        [HtmlAttributeName(FormatAttributeName)]
        public DateInputFormat Format { get; set; }

        public DateInputAutocomplete? Autocomplete { get; set; }

        public string? Name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);
            output.TagName = "gcds-date-input";
            output.TagMode = TagMode.StartTagAndEndTag;
            if (Autocomplete != null)
            {
                output.Attributes.SetAttribute("autocomplete", Autocomplete.ToString());
            }
            switch (Format)
            {
                case DateInputFormat.compact:
                    output.Attributes.SetAttribute("format", "compact");
                    break;
                case DateInputFormat.full:
                    output.Attributes.SetAttribute("format", "full");
                    break;
                case DateInputFormat.iso:
                    output.Attributes.SetAttribute("format", "iso");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(FormatAttributeName, Format, null);
            }
            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;
            if (metadata == null)
            {
                throw new InvalidOperationException(ResourceFormat.FormatTagHelpers_NoProvidedMetadata(
                "<gcds-date-input>",
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
                htmlAttributes.Add("name", Name);
            }
            else
            {
                htmlAttributes.Add("name", For.Name);
            }
            TagBuilder tagBuilder = Generator.GenerateDateInput(
                ViewContext,
                modelExplorer,
                For.Name,
                For.Model,
                htmlAttributes);
            if (tagBuilder != null)
            {
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

    public enum DateInputFormat
    {
        compact = 0,
        full = 1,
        iso = 2
    }

    public sealed class DateInputAutocomplete
    {
        private readonly string name;
        private readonly int value;

        private static readonly Dictionary<string, DateInputAutocomplete> map = [];

        private DateInputAutocomplete(int value, string name)
        {
            this.value = value;
            this.name = name;
            map[name] = this;
        }

        public static readonly DateInputAutocomplete bday = new(0, "bday");
        public static readonly DateInputAutocomplete ccexp = new(1, "cc-exp");
        public static readonly DateInputAutocomplete off = new(2, "off");
        public static readonly DateInputAutocomplete on = new(3, "on");

        public override string ToString() => name;


    }

    //public enum DateInputAutocomplete
    //{
    //    bday = 0,
    //    ccexp = 1,
    //    off = 2,
    //    on = 3
    //}
}
