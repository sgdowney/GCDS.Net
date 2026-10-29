using Gcds.Net.Models;
using Gcds.Net.Properties;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Text.Encodings.Web;

namespace Gcds.Net.Renderer
{
    public class GcdsHtmlGenerator(IAntiforgery antiforgery, IOptions<MvcViewOptions> optionsAccessor, IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder, ValidationHtmlAttributeProvider validationAttributeProvider) : DefaultHtmlGenerator(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider), IHtmlGenerator
    {
        private readonly IModelMetadataProvider _metadataProvider = metadataProvider;
        private readonly FormInputRenderMode _formInputRenderMode = optionsAccessor.Value.HtmlHelperOptions.FormInputRenderMode;

        public override TagBuilder GenerateTextBox(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, string format, object htmlAttributes)
        {
            ArgumentNullException.ThrowIfNull(viewContext, nameof(viewContext));

            var htmlAttributeDictionary = GetHtmlAttributeDictionaryOrNull(htmlAttributes);

            TagBuilder tagBuilder = GenerateGcdsInput(viewContext, InputTypeEnum.text, modelExplorer, expression, value, useViewData: (modelExplorer == null && value == null),
            isChecked: false,
            setId: true,
            isExplicitValue: true,
            format: format,
            htmlAttributes: htmlAttributeDictionary!);
            return tagBuilder;

        }

        public override TagBuilder GeneratePassword(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, object htmlAttributes)
        {
            ArgumentNullException.ThrowIfNull(viewContext);

            var htmlAttributeDictionary = GetHtmlAttributeDictionaryOrNull(htmlAttributes);
            return GenerateGcdsInput(
                viewContext,
                InputTypeEnum.password,
                modelExplorer,
                expression,
                value,
                useViewData: false,
                isChecked: false,
                setId: true,
                isExplicitValue: true,
                format: null,
                htmlAttributes: htmlAttributeDictionary!);
        }


        protected virtual TagBuilder GenerateGcdsInput(ViewContext viewContext, InputTypeEnum inputType, ModelExplorer modelExplorer, string expression, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string? format, IDictionary<string, object> htmlAttributes)
        {
            ArgumentNullException.ThrowIfNull(viewContext, nameof(viewContext));

            var fullName = GcdsNameAndIdProvider.GetFullHtmlFieldName(viewContext, expression);

            if (!IsFullNameValid(fullName, htmlAttributes))
            {
                throw new ArgumentException("Name is not valid.");
            }

            var inputTypeString = GetInputTypeString(inputType);

            var tagBuilder = new TagBuilder("gcds-input");

            tagBuilder.MergeAttributes(htmlAttributes!);
            tagBuilder.MergeAttribute("type", inputTypeString);
            if (!string.IsNullOrEmpty(fullName))
            {
                tagBuilder.MergeAttribute("name", fullName, replaceExisting: true);
            }
            var suppliedTypeString = tagBuilder.Attributes["type"];

            CultureInfo culture;
            if (ShouldUseInvariantFormattingForInputType(suppliedTypeString!, viewContext.Html5DateRenderingMode))
            {
                culture = CultureInfo.InvariantCulture;
            }
            else
            {
                culture = CultureInfo.CurrentCulture;
            }

            var valueParameter = FormatValue(value, format!, culture);

            switch (inputType)
            {
                case InputTypeEnum.password:
                    if (value != null)
                    {
                        tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
                    }

                    break;

                case InputTypeEnum.text:
                default:

                    string? attributeValue = GetModelStateValue(viewContext, fullName, typeof(string)) as string;
                    attributeValue ??= useViewData ? EvalString(viewContext, expression, format!) : valueParameter;
                    tagBuilder.MergeAttribute("value", attributeValue, replaceExisting: isExplicitValue);

                    break;
            }

            if (setId)
            {
                GcdsNameAndIdProvider.GenerateId(viewContext, tagBuilder, "input-id", fullName, IdAttributeDotReplacement);
            }

            string? labelText = null;
            if (htmlAttributes.TryGetValue("label", out var label))
            {
                labelText = label.ToString()!;
            }
            string? resolvedLabelText = ResolveLabelText(labelText, modelExplorer, expression);
            if (resolvedLabelText != null)
            {
                tagBuilder.MergeAttribute("label", resolvedLabelText);
            }

            AddValidationAttributes(viewContext, tagBuilder, modelExplorer, expression);

            return tagBuilder;

        }


        public override TagBuilder GenerateSelect(
            ViewContext viewContext,
            ModelExplorer modelExplorer,
            string optionLabel,
            string expression,
            IEnumerable<SelectListItem> selectList,
            ICollection<string> currentValues,
            bool allowMultiple,
            object htmlAttributes)
        {
            ArgumentNullException.ThrowIfNull(viewContext);

            var fullName = GcdsNameAndIdProvider.GetFullHtmlFieldName(viewContext, expression);
            var htmlAttributeDictionary = GetHtmlAttributeDictionaryOrNull(htmlAttributes);
            if (!IsFullNameValid(fullName, htmlAttributeDictionary!))
            {
                throw new ArgumentException(
                    ResourceFormat.FormatHtmlGenerator_FieldNameCannotBeNullOrEmpty(
                        typeof(IHtmlHelper).FullName,
                        nameof(IHtmlHelper.Editor),
                        typeof(IHtmlHelper<>).FullName,
                        nameof(IHtmlHelper<object>.EditorFor),
                        "htmlFieldName"),
                    nameof(expression));
            }

            // If we got a null selectList, try to use ViewData to get the list of items.
            selectList ??= GetSelectListItems(viewContext, expression);

            modelExplorer ??= ExpressionMetadataProvider.FromStringExpression(expression, viewContext.ViewData, _metadataProvider);

            // Convert each ListItem to an <option> tag and wrap them with <optgroup> if requested.
            var listItemBuilder = GenerateGroupsAndOptions(optionLabel, selectList, currentValues);

            var tagBuilder = new TagBuilder("gcds-select");
            tagBuilder.InnerHtml.SetHtmlContent(listItemBuilder);
            tagBuilder.MergeAttributes(htmlAttributeDictionary!);
            GcdsNameAndIdProvider.GenerateId(viewContext, tagBuilder, "select-id", fullName, IdAttributeDotReplacement);
            if (!string.IsNullOrEmpty(fullName))
            {
                tagBuilder.MergeAttribute("name", fullName, replaceExisting: true);
            }

            if (allowMultiple)
            {
                tagBuilder.MergeAttribute("multiple", "multiple");
            }

            string? labelText = null;
            if (htmlAttributeDictionary!.TryGetValue("label", out var label))
            {
                labelText = label.ToString()!;
            }
            string? resolvedLabelText = ResolveLabelText(labelText, modelExplorer, expression);
            if (resolvedLabelText != null)
            {
                tagBuilder.MergeAttribute("label", resolvedLabelText);
            }

            AddValidationAttributes(viewContext, tagBuilder, modelExplorer, expression);

            return tagBuilder;
        }


        private static IEnumerable<SelectListItem> GetSelectListItems(ViewContext viewContext, string expression)
        {
            ArgumentNullException.ThrowIfNull(viewContext);

            // Method is called only if user did not pass a select list in. They must provide select list items in the
            // ViewData dictionary and definitely not as the Model. (Even if the Model datatype were correct, a
            // <select> element generated for a collection of SelectListItems would be useless.)
            var value = viewContext.ViewData.Eval(expression);

            // First check whether above evaluation was successful and did not match ViewData.Model.
            if (value == null || value == viewContext.ViewData.Model)
            {
                throw new InvalidOperationException(ResourceFormat.FormatHtmlHelper_MissingSelectData(
                    $"IEnumerable<{nameof(SelectListItem)}>",
                    expression));
                //throw new InvalidOperationException("Missing Select data.");
            }

            // Second check the Eval() call returned a collection of SelectListItems.
            if (value is not IEnumerable<SelectListItem> selectList)
            {
                throw new InvalidOperationException(ResourceFormat.FormatHtmlHelper_WrongSelectDataType(
                    expression,
                    value.GetType().FullName,
                    $"IEnumerable<{nameof(SelectListItem)}>"));
                //throw new InvalidOperationException("Wrong Select data type.");
            }

            return selectList;
        }



        // Only need a dictionary if htmlAttributes is non-null. TagBuilder.MergeAttributes() is fine with null.
        private static IDictionary<string, object>? GetHtmlAttributeDictionaryOrNull(object htmlAttributes)
        {
            IDictionary<string, object>? htmlAttributeDictionary = null;
            if (htmlAttributes != null)
            {
                htmlAttributeDictionary = htmlAttributes as IDictionary<string, object>;
                htmlAttributeDictionary ??= Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            }

            return htmlAttributeDictionary;
        }

        private static bool IsFullNameValid(string fullName, IDictionary<string, object> htmlAttributeDictionary)
        {
            return IsFullNameValid(fullName, htmlAttributeDictionary, fallbackAttributeName: "name");
        }

        private static bool IsFullNameValid(
            string fullName,
            IDictionary<string, object> htmlAttributeDictionary,
            string fallbackAttributeName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                // fullName==null is normally an error because name="" is not valid in HTML 5.
                if (htmlAttributeDictionary == null)
                {
                    return false;
                }

                // Check if user has provided an explicit name attribute.
                // Generalized a bit because other attributes e.g. data-valmsg-for refer to element names.
                htmlAttributeDictionary.TryGetValue(fallbackAttributeName, out var attributeObject);
                var attributeString = Convert.ToString(attributeObject, CultureInfo.InvariantCulture);
                if (string.IsNullOrEmpty(attributeString))
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetInputTypeString(InputTypeEnum inputType)
        {
            return inputType switch
            {
                InputTypeEnum.email => "email",
                InputTypeEnum.number => "number",
                InputTypeEnum.password => "password",
                InputTypeEnum.search => "search",
                InputTypeEnum.tel => "tel",
                InputTypeEnum.url => "url",
                InputTypeEnum.text => "text",
                _ => "text",
            };
        }


        private static string? ResolveLabelText(string? labelText, ModelExplorer modelExplorer, string? expression)
        {
            var resolvedLabelText = labelText ?? modelExplorer.Metadata.DisplayName ?? modelExplorer.Metadata.PropertyName;
            if (resolvedLabelText == null && expression != null)
            {
                var index = expression.LastIndexOf('.');
                if (index == -1)
                {
                    // Expression does not contain a dot separator.
                    resolvedLabelText = expression;
                }
                else
                {
                    resolvedLabelText = expression[(index + 1)..];
                }
            }
            return resolvedLabelText;
        }


        private bool ShouldUseInvariantFormattingForInputType(string inputType, Html5DateRenderingMode dateRenderingMode)
        {
            if (_formInputRenderMode == FormInputRenderMode.DetectCultureFromInputType)
            {
                var isNumberInput =
                    string.Equals(inputType, "number", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(inputType, "range", StringComparison.OrdinalIgnoreCase);

                if (isNumberInput)
                {
                    return true;
                }

                if (dateRenderingMode != Html5DateRenderingMode.CurrentCulture)
                {
                    var isDateInput =
                        string.Equals(inputType, "date", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(inputType, "datetime-local", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(inputType, "month", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(inputType, "time", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(inputType, "week", StringComparison.OrdinalIgnoreCase);

                    if (isDateInput)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static string FormatValue(object value, string format, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(format))
            {
                return Convert.ToString(value, formatProvider)!;
            }
            else
            {
                return string.Format(formatProvider, format, value);
            }

        }

        internal static object? GetModelStateValue(ViewContext viewContext, string key, Type destinationType)
        {
            if (viewContext.ViewData.ModelState.TryGetValue(key, out var entry) && entry.RawValue != null)
            {
                //return ModelBindingHelper.ConvertTo(entry.RawValue, destinationType, culture: null);
                return ConvertTo(entry.RawValue, destinationType, culture: null);
            }

            return null;
        }


        private static object? ConvertTo(object? value, Type type, CultureInfo? culture)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (value == null)
            {
                // For value types, treat null values as though they were the default value for the type.
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            if (type.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            var cultureToUse = culture ?? CultureInfo.InvariantCulture;
            return UnwrapPossibleArrayType(value, type, cultureToUse);
        }

        private static object? UnwrapPossibleArrayType(object value, Type destinationType, CultureInfo culture)
        {
            // array conversion results in four cases, as below
            var valueAsArray = value as Array;
            if (destinationType.IsArray)
            {
                var destinationElementType = destinationType.GetElementType()!;
                if (valueAsArray != null)
                {
                    // case 1: both destination + source type are arrays, so convert each element
                    var converted = (IList)Array.CreateInstance(destinationElementType, valueAsArray.Length);
                    for (var i = 0; i < valueAsArray.Length; i++)
                    {
                        converted[i] = ConvertSimpleType(valueAsArray.GetValue(i), destinationElementType, culture);
                    }
                    return converted;
                }
                else
                {
                    // case 2: destination type is array but source is single element, so wrap element in
                    // array + convert
                    var element = ConvertSimpleType(value, destinationElementType, culture);
                    var converted = (IList)Array.CreateInstance(destinationElementType, 1);
                    converted[0] = element;
                    return converted;
                }
            }
            else if (valueAsArray != null)
            {
                // case 3: destination type is single element but source is array, so extract first element + convert
                if (valueAsArray.Length > 0)
                {
                    var elementValue = valueAsArray.GetValue(0);
                    return ConvertSimpleType(elementValue, destinationType, culture);
                }
                else
                {
                    // case 3(a): source is empty array, so can't perform conversion
                    return null;
                }
            }

            // case 4: both destination + source type are single elements, so convert
            return ConvertSimpleType(value, destinationType, culture);
        }

        private static object? ConvertSimpleType(object? value, Type destinationType, CultureInfo culture)
        {
            if (value == null || destinationType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            // In case of a Nullable object, we try again with its underlying type.
            destinationType = UnwrapNullableType(destinationType);

            // if this is a user-input value but the user didn't type anything, return no value
            if (value is string valueAsString && string.IsNullOrWhiteSpace(valueAsString))
            {
                return null;
            }

            var converter = TypeDescriptor.GetConverter(destinationType);
            var canConvertFrom = converter.CanConvertFrom(value.GetType());
            if (!canConvertFrom)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!(canConvertFrom || converter.CanConvertTo(destinationType)))
            {
                // EnumConverter cannot convert integer, so we verify manually
                if (destinationType.IsEnum &&
                    (value is int ||
                    value is uint ||
                    value is long ||
                    value is ulong ||
                    value is short ||
                    value is ushort ||
                    value is byte ||
                    value is sbyte))
                {
                    return Enum.ToObject(destinationType, value);
                }
                //throw new InvalidOperationException($"No converter exists. {value.GetType()} => {destinationType}");
                throw new InvalidOperationException(ResourceFormat.FormatValueProviderResult_NoConverterExists(value.GetType(), destinationType));
            }

            try
            {
                return canConvertFrom
                    ? converter.ConvertFrom(null, culture, value)
                    : converter.ConvertTo(null, culture, value, destinationType);
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw;
                }
                else
                {
                    // TypeConverter throws System.Exception wrapping the FormatException,
                    // so we throw the inner exception.
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();

                    // This code is never reached because the previous line will always throw.
                    throw;
                }
            }
        }

        private static Type UnwrapNullableType(Type destinationType)
        {
            return Nullable.GetUnderlyingType(destinationType) ?? destinationType;
        }

        internal static string EvalString(ViewContext viewContext, string key, string format)
        {
            return Convert.ToString(viewContext.ViewData.Eval(key, format), CultureInfo.CurrentCulture)!;
        }

        private static IHtmlContent GenerateGroupsAndOptions(
            string optionLabel,
            IEnumerable<SelectListItem> selectList,
            ICollection<string> currentValues)
        {
            if (selectList is not IList<SelectListItem> itemsList)
            {
                itemsList = selectList.ToList();
            }

            var count = itemsList.Count;
            if (optionLabel != null)
            {
                count++;
            }

            // Short-circuit work below if there's nothing to add.
            if (count == 0)
            {
                return HtmlString.Empty;
            }

            var listItemBuilder = new HtmlContentBuilder(count);

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
            {
                listItemBuilder.AppendLine(GenerateOption(
                    new SelectListItem()
                    {
                        Text = optionLabel,
                        Value = string.Empty,
                        Selected = false,
                    },
                    currentValues: null));
            }

            // Group items in the SelectList if requested.
            // The worst case complexity of this algorithm is O(number of groups*n).
            // If there aren't any groups, it is O(n) where n is number of items in the list.
            var optionGenerated = new bool[itemsList.Count];
            for (var i = 0; i < itemsList.Count; i++)
            {
                if (!optionGenerated[i])
                {
                    var item = itemsList[i];
                    var optGroup = item.Group;
                    if (optGroup != null)
                    {
                        var groupBuilder = new TagBuilder("optgroup");
                        if (optGroup.Name != null)
                        {
                            groupBuilder.MergeAttribute("label", optGroup.Name);
                        }

                        if (optGroup.Disabled)
                        {
                            groupBuilder.MergeAttribute("disabled", "disabled");
                        }

                        groupBuilder.InnerHtml.AppendLine();

                        for (var j = i; j < itemsList.Count; j++)
                        {
                            var groupItem = itemsList[j];

                            if (!optionGenerated[j] &&
                                object.ReferenceEquals(optGroup, groupItem.Group))
                            {
                                groupBuilder.InnerHtml.AppendLine(GenerateOption(groupItem, currentValues));
                                optionGenerated[j] = true;
                            }
                        }

                        listItemBuilder.AppendLine(groupBuilder);
                    }
                    else
                    {
                        listItemBuilder.AppendLine(GenerateOption(item, currentValues));
                        optionGenerated[i] = true;
                    }
                }
            }

            return listItemBuilder;
        }

        private static IHtmlContent GenerateOption(SelectListItem item, ICollection<string>? currentValues)
        {
            var selected = item.Selected;
            if (currentValues != null)
            {
                var value = item.Value ?? item.Text;
                selected = currentValues.Contains(value);
            }

            var tagBuilder = GenerateOption(item, item.Text, selected);
            return tagBuilder;
        }

        internal static TagBuilder GenerateOption(SelectListItem item, string text, bool selected)
        {
            var tagBuilder = new TagBuilder("option");
            tagBuilder.InnerHtml.SetContent(text);

            if (item.Value != null)
            {
                tagBuilder.Attributes["value"] = item.Value;
            }

            if (selected)
            {
                tagBuilder.Attributes["selected"] = "selected";
            }

            if (item.Disabled)
            {
                tagBuilder.Attributes["disabled"] = "disabled";
            }

            return tagBuilder;
        }

    }
}
