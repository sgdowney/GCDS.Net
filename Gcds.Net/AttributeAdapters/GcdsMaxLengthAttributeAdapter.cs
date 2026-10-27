using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Gcds.Net.AttributeAdapters
{
    public class GcdsMaxLengthAttributeAdapter : AttributeAdapterBase<MaxLengthAttribute>
    {
        private readonly string _maxLength;

        public GcdsMaxLengthAttributeAdapter(MaxLengthAttribute attribute, IStringLocalizer? stringLocalizer) : base(attribute, stringLocalizer)
        {
            _maxLength = Attribute.Length.ToString(CultureInfo.InvariantCulture);
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            MergeAttribute(context.Attributes, "maxlength", _maxLength);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), _maxLength);
        }
    }
}
