using Gcds.Net.Extensions;
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

    [HtmlTargetElement("gcds-input", Attributes = ForAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GcdsInputTagHelper(IHtmlGenerator Generator) : TagHelper
    {

        private const string ForAttributeName = "gcds-for";

        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="InputTagHelper"/>'s output.
        /// </summary>
        //protected IHtmlGenerator Generator { get; }


        [HtmlAttributeName("hint")]
        public string? Hint { get; set; }

        [HtmlAttributeName("autocomplete")]
        public string? Autocomplete { get; set; }

        [HtmlAttributeName("autofocus")]
        public bool Autofocus { get; set; } = false;

        [HtmlAttributeName("size")]
        public int? Size { get; set; }

        [HtmlAttributeName("form")]
        public string? FormName { get; set; }

        [HtmlAttributeName("disabled")]
        public bool Disabled { get; set; } = false;

        [HtmlAttributeName("readonly")]
        public bool ReadOnly { get; set; } = false;

        [HtmlAttributeNotBound]
        [ViewContext]
        public required ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public required ModelExpression For { get; set; }

        [HtmlAttributeName("type")]
        public InputTypeEnum InputType { get; set; }

        [HtmlAttributeName("label")]
        public string? Label { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        [HtmlAttributeName("input-id")]
        public required string InputId { get; set; }

        [HtmlAttributeName("lang")]
        public PageLanguage? Language { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(output);

            // Pass through attributes that are also well-known HTML attributes. Must be done prior to any copying
            // from a TagBuilder.
            //output.CopyHtmlAttribute("type", context);
            output.Attributes.Add("type", InputType);

            if (Name != null)
            {
                output.CopyHtmlAttribute("name", context);
            }

            if (Value != null)
            {
                output.CopyHtmlAttribute("value", context);
            }

            if (FormName != null)
            {
                output.CopyHtmlAttribute("form", context);
            }

            if (Size != null)
            {
                output.CopyHtmlAttribute("size", context);
            }

            if (Hint != null)
            {
                output.CopyHtmlAttribute("hint", context);
            }

            if (Disabled)
            {
                //output.CopyHtmlAttribute(nameof(Disabled), context);
                output.Attributes.Add(new TagHelperAttribute("disabled", value: null, valueStyle: HtmlAttributeValueStyle.Minimized));
            }

            if (ReadOnly)
            {
                //output.CopyHtmlAttribute(nameof(ReadOnly), context);
                output.Attributes.Add(new TagHelperAttribute("readonly", value: null, valueStyle: HtmlAttributeValueStyle.Minimized));
            }

            if (Autocomplete != null)
            {
                output.CopyHtmlAttribute("autocomplete", context);
            }

            if (Autofocus)
            {
                output.Attributes.Add(new TagHelperAttribute("autofocus", value: null, valueStyle: HtmlAttributeValueStyle.Minimized));
            }

            if (Label != null)
            {
                output.CopyHtmlAttribute("label", context);
            }

            if (InputId != null)
            {
                output.CopyHtmlAttribute("input-id", context);
            }

            if (Language != null)
            {
                output.CopyHtmlAttribute("lang", context);
            }
            //else
            //{
            //    output.Attributes.Add(new TagHelperAttribute("lang", PageLanguage.en));
            //}

            // Note null or empty For.Name is allowed because TemplateInfo.HtmlFieldPrefix may be sufficient.
            // IHtmlGenerator will enforce name requirements.
            var metadata = For.Metadata;
            var modelExplorer = For.ModelExplorer;
            if (metadata == null)
            {
                //throw new InvalidOperationException($"Missing metadata in {ForAttributeName} for {For.Name}.");
                throw new InvalidOperationException(ResourceFormat.FormatTagHelpers_NoProvidedMetadata(
                "<gcds-input>",
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
                //htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                //{
                //    { "name", Name },
                //};
                htmlAttributes["name"] = Name;
            }
            else
            {
                htmlAttributes.Add("name", For.Name);
            }
            //if (!string.IsNullOrEmpty(Label)) 
            //{
            //    htmlAttributes.Add("label", Label);
            //}
            TagBuilder tagBuilder;
            switch (InputType)
            {
                //case "hidden":
                //    tagBuilder = GenerateHidden(modelExplorer, htmlAttributes);
                //    break;

                //case "checkbox":
                //    tagBuilder = GenerateCheckBox(modelExplorer, output, htmlAttributes);
                //    break;

                case InputTypeEnum.password:
                    tagBuilder = Generator.GeneratePassword(
                        ViewContext,
                        modelExplorer,
                        For.Name,
                        value: null,
                        htmlAttributes: htmlAttributes);
                    break;

                //case "radio":
                //    tagBuilder = GenerateRadio(modelExplorer, htmlAttributes);
                //    break;
                case InputTypeEnum.email:
                case InputTypeEnum.text:    
                default:
                    tagBuilder = GenerateTextBox(modelExplorer, InputType, htmlAttributes);
                    break;
            }
            if (tagBuilder != null)
            {
                if (tagBuilder.Attributes.TryGetValue("required", out _))
                {
                    output.Attributes.Add(new TagHelperAttribute("required", value: null, valueStyle: HtmlAttributeValueStyle.Minimized));
                }

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

        private TagBuilder GenerateTextBox(ModelExplorer modelExplorer,  InputTypeEnum inputType, IDictionary<string, object> htmlAttributes)
        {

            //if (htmlAttributes == null)
            //{
            //    htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            //}

            htmlAttributes["type"] = inputType;
            //if (string.Equals(inputType, "file") &&
            //    string.Equals(
            //        inputTypeHint,
            //        TemplateRenderer.IEnumerableOfIFormFileName,
            //        StringComparison.OrdinalIgnoreCase))
            //{
            //    htmlAttributes["multiple"] = "multiple";
            //}

            return Generator.GenerateTextBox(
                ViewContext,
                modelExplorer,
                For.Name,
                modelExplorer.Model,
                null,
                htmlAttributes);
        }


        //public override void Process(TagHelperContext context, TagHelperOutput output)
        //{

        //    ArgumentNullException.ThrowIfNull(context);
        //    ArgumentNullException.ThrowIfNull(output);

        //    IInputModel inputModel = new InputModel
        //    {
        //        Hint = Hint,
        //        Size = Size,
        //        Form = Form,
        //        AutoFocus = Autofocus,
        //        AutoComplete = Autocomplete,
        //        Disabled = Disabled,
        //        ReadOnly = ReadOnly,
        //        InputType = InputType
        //    };
        //    //https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.TagHelpers/src/InputTagHelper.cs#L247
        //    inputModelConfig.BuildInputModel(ViewContext, For, inputModel);

        //    TagBuilder tagBuilder = (TagBuilder)gcdsRenderer.RenderInput(inputModel);
        //    if (tagBuilder != null)
        //    {
        //        output.MergeAttributes(tagBuilder);
        //        // Do not update the content if another tag helper targeting this element has already done so.
        //        if (!output.IsContentModified && tagBuilder.HasInnerHtml)
        //        {
        //            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        //        }
        //    }
        //}

        //public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    return Task.Run(()=>Process(context, output));
        //}
    }
}
