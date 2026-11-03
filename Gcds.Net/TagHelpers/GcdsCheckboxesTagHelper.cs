using Gcds.Net.Models;
using Gcds.Net.Properties;
using Gcds.Net.Renderer;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;

using System.Text.Json;

namespace Gcds.Net.TagHelpers
{
    [HtmlTargetElement("gcds-checkboxes", Attributes = ItemsAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("gcds-checkboxes", Attributes = ForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GcdsCheckboxesTagHelper(IGcdsHtmlGenerator generator) : TagHelper
    {
        private const string ForAttributeName = "gcds-for";
        private const string ItemsAttributeName = "options";
        private ICollection<string>? _currentValues;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };

        [HtmlAttributeNotBound]
        [ViewContext]
        public required ViewContext ViewContext { get; set; }

        [HtmlAttributeName("name")]
        public string? Name { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public required ModelExpression For { get; set; }

        [HtmlAttributeName(ItemsAttributeName)]
        public required IEnumerable<ICheckboxRadioItemModel> Items { get; set; }
        
        protected IGcdsHtmlGenerator Generator { get; } = generator;

        public override void Init(TagHelperContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            if (For == null)
            {
                return;
            }

            if (For.Metadata == null)
            {
                throw new InvalidOperationException(ResourceFormat.FormatTagHelpers_NoProvidedMetadata("<gcds-checkboxes>", ForAttributeName, nameof(IModelMetadataProvider), For.Name));
            }
            _currentValues = Generator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, true);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (Items == null)
            {
                throw new InvalidOperationException(ResourceFormat.FormatTagHelpers_NoProvidedMetadata("<gcds-checkboxes>", ItemsAttributeName, nameof(IEnumerable<ICheckboxRadioItemModel>), "Items"));
            }
            else
            {
                //output.Attributes.Add(new TagHelperAttribute("options", JsonSerializer.Serialize(Items), HtmlAttributeValueStyle.SingleQuotes));
                output.Attributes.SetAttribute("options", JsonSerializer.Serialize(Items, _jsonSerializerOptions));
            }

            if (Name != null)
            {
                output.CopyHtmlAttribute(nameof(Name), context);
            }

            if (For == null)
            {
                return;
            }

            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(For.Name) &&
                string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
                !string.IsNullOrEmpty(Name))
            {
                htmlAttributes.Add("name", Name);
            }

            var tagBuilder = Generator.GenerateCheckboxes(ViewContext, For.ModelExplorer, For.Name, htmlAttributes);
            if (tagBuilder != null)
            {
                output.MergeAttributes(tagBuilder);
                if (tagBuilder.HasInnerHtml)
                {
                    output.PostContent.AppendHtml(tagBuilder.InnerHtml);
                }
            }
            if (_currentValues != null && _currentValues!.Count > 0) 
            { 
                output.Attributes.SetAttribute("value", JsonSerializer.Serialize(_currentValues));
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
