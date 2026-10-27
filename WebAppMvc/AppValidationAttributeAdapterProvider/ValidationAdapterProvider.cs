using Gcds.Net.AttributeAdapters;

using GoC.Components.DataAnnotations.Attributes;
using GoC.Components.DataAnnotations.Provider;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

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
                if (attribute.GetType().IsAssignableTo(typeof(SINAttribute)))
                {
                    IOptions<WetValidationAttributeAdapterProviderOptions> options = Options.Create(new WetValidationAttributeAdapterProviderOptions { UseWebExperienceToolkit = true });
                    WetValidationAttributeAdapterProvider gocProvider = new(options);
                    attributeAdapter = gocProvider.GetAttributeAdapter(attribute, stringLocalizer);
                }
            }
            //if (attributeAdapter == null)
            //{
            //    ValidationAttributeAdapterProvider baseProvider = new();
            //    attributeAdapter = baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            //}
            return attributeAdapter;
        }
    }
}
