using Gcds.Net.Properties;
using Gcds.Net.Renderer;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System.Collections;

namespace Gcds.Net.TagHelpers
{
    [HtmlTargetElement("gcds-select", Attributes = ForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("gcds-select", Attributes = ItemsAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GcdsSelectTagHelper(IGcdsHtmlGenerator generator) : TagHelper
    {
        private const string ForAttributeName = "gcds-for";
        private const string ItemsAttributeName = "gcds-options";
        private bool _allowMultiple;
        private ICollection<string>? _currentValues;

        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="SelectTagHelper"/>'s output.
        /// </summary>
        protected IGcdsHtmlGenerator Generator { get; } = generator;

        [HtmlAttributeName("name")]
        public string? Name { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public required ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public required ModelExpression For { get; set; }

        [HtmlAttributeName(ItemsAttributeName)]
        public required IEnumerable<SelectListItem> Items { get; set; }

        public override void Init(TagHelperContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (For == null)
            {
                // Informs contained elements that they're running within a targeted <select/> element.
                context.Items[typeof(GcdsSelectTagHelper)] = null;
                return;
            }


            // Note null or empty For.Name is allowed because TemplateInfo.HtmlFieldPrefix may be sufficient.
            // IHtmlGenerator will enforce name requirements.
            if (For.Metadata == null)
            {
                //throw new InvalidOperationException($"Missing metadata in {ForAttributeName} for model item {For.Name}.");
                throw new InvalidOperationException(ResourceFormat.FormatTagHelpers_NoProvidedMetadata(
                    "<gcds-select>",
                    ForAttributeName,
                    nameof(IModelMetadataProvider),
                    For.Name));
            }

            // Base allowMultiple on the instance or declared type of the expression to avoid a
            // "SelectExpressionNotEnumerable" InvalidOperationException during generation.
            // Metadata.IsEnumerableType is similar but does not take runtime type into account.
            var realModelType = For.ModelExplorer.ModelType;
            _allowMultiple = typeof(string) != realModelType &&
                typeof(IEnumerable).IsAssignableFrom(realModelType);
            _currentValues = Generator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, _allowMultiple);

            // Whether or not (not being highly unlikely) we generate anything, could update contained <option/>
            // elements. Provide selected values for <option/> tag helpers.
            var currentValues = _currentValues == null ? null : new CurrentValues(_currentValues);
            context.Items[typeof(GcdsSelectTagHelper)] = currentValues;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);

            // Pass through attribute that is also a well-known HTML attribute. Must be done prior to any copying
            // from a TagBuilder.
            if (Name != null)
            {
                output.CopyHtmlAttribute(nameof(Name), context);
            }

            // Ensure GenerateSelect() _never_ looks anything up in ViewData.
            IEnumerable<SelectListItem> items = Items ?? [];

            if (For == null)
            {
                var options = Generator.GenerateGroupsAndOptions(optionLabel: null, selectList: items);
                output.PostContent.AppendHtml(options);
                return;
            }

            // Ensure Generator does not throw due to empty "fullName" if user provided a name attribute.
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(For.Name) &&
                string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
                !string.IsNullOrEmpty(Name))
            {
                htmlAttributes.Add("name", Name);
            }

            var tagBuilder = Generator.GenerateSelect(
                ViewContext,
                For.ModelExplorer,
                optionLabel: null,
                expression: For.Name,
                selectList: items,
                currentValues: _currentValues,
                allowMultiple: _allowMultiple,
                htmlAttributes: htmlAttributes);

            if (tagBuilder != null)
            {
                output.MergeAttributes(tagBuilder);
                if (tagBuilder.HasInnerHtml)
                {
                    output.PostContent.AppendHtml(tagBuilder.InnerHtml);
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
