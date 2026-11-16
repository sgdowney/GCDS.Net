using Gcds.Net.AttributeAdapters;

using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.AppValidationAttributeAdapterProvider
{
    public class ValidationAdapterProvider() : IValidationAttributeAdapterProvider
    {
        public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
        {
            GcdsValidationAttributeAdapterProvider gcdsProvider = new();
            IAttributeAdapter? attributeAdapter = gcdsProvider.GetAttributeAdapter(attribute, stringLocalizer);
            if (attributeAdapter == null) 
            {
                // Provide custom attribute handling for attributes not handled by GcdsValidationAttributeAdapterProvider. 
                // You can create your own attribute adapters and call them here if needed. This works well for custom
                // validation attributes that can be mapped to existing GCDS validation attributes.
                // For example, if you have a custom attribute that is similar to RequiredAttribute, you can create a custom
                // adapter for it and return it here.
            }
            return attributeAdapter;
        }
    }
}
