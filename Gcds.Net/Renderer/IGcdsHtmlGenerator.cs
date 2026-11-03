using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gcds.Net.Renderer
{
    public interface IGcdsHtmlGenerator : IHtmlGenerator
    {
        TagBuilder GenerateCheckboxes(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object htmlAttributes);
        TagBuilder GenerateRadios(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object htmlAttributes);
        new TagBuilder GeneratePassword(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object? value, object htmlAttributes);
        new TagBuilder GenerateSelect(ViewContext viewContext, ModelExplorer modelExplorer, string? optionLabel, string expression, IEnumerable<SelectListItem> selectList, ICollection<string> currentValues, bool allowMultiple, object htmlAttributes);
        new TagBuilder GenerateTextBox(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, string? format, object htmlAttributes);
    }
}