using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;

namespace Gcds.Net.AttributeAdapters
{
    public class GcdsReqularExpressionAttributeAdapter(RegularExpressionAttribute attribute, IStringLocalizer? stringLocalizer) : AttributeAdapterBase<RegularExpressionAttribute>(attribute, stringLocalizer)
    {
        public override void AddValidation(ClientModelValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            MergeAttribute(context.Attributes, "pattern", Attribute.Pattern);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Pattern);

        }
    }
}
