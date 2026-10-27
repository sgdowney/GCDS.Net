using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;

namespace Gcds.Net.AttributeAdapters
{
    public class GcdsValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
        {
            ArgumentNullException.ThrowIfNull(attribute, nameof(attribute));
            IAttributeAdapter? attributeAdapter = null;
            var type = attribute.GetType();
            if (typeof(RequiredAttribute).IsAssignableFrom(type))
            {
                attributeAdapter = new GcdsRequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);
            }
            else if (typeof(MinLengthAttribute).IsAssignableFrom(type))
            {
                attributeAdapter = new GcdsMinLengthAttributeAdapter((MinLengthAttribute)attribute, stringLocalizer);
            }
            else if (typeof(MaxLengthAttribute).IsAssignableFrom(type))
            {
                attributeAdapter = new GcdsMaxLengthAttributeAdapter((MaxLengthAttribute)attribute, stringLocalizer);
            }
            else if (typeof(RegularExpressionAttribute).IsAssignableFrom(type))
            {
                attributeAdapter = new GcdsReqularExpressionAttributeAdapter((RegularExpressionAttribute)attribute, stringLocalizer);
            }
            return attributeAdapter;
        }
    }
}
