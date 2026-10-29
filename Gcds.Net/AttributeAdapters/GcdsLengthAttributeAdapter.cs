using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Gcds.Net.AttributeAdapters
{
    public class GcdsLengthAttributeAdapter : AttributeAdapterBase<LengthAttribute>
    {
        private readonly string _maxLength;
        private readonly string _minLength;

        public GcdsLengthAttributeAdapter(LengthAttribute attribute, IStringLocalizer? stringLocalizer) : base(attribute, stringLocalizer)
        {
            _maxLength = Attribute.MaximumLength.ToString(CultureInfo.InvariantCulture);
            _minLength = Attribute.MinimumLength.ToString(CultureInfo.InvariantCulture);
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            MergeAttribute(context.Attributes, "maxlength", _maxLength);
            MergeAttribute(context.Attributes, "minlength", _minLength);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.MaximumLength,
                Attribute.MinimumLength);
        }
    }
}
