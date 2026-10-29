using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Gcds.Net.AttributeAdapters
{
    public class GcdsRangeAttributeAdapter : AttributeAdapterBase<RangeAttribute>
    {
        private readonly string _max;
        private readonly string _min;

        public GcdsRangeAttributeAdapter(RangeAttribute attribute, IStringLocalizer? stringLocalizer) : base(attribute, stringLocalizer)
        {
            // This will trigger the conversion of Attribute.Minimum and Attribute.Maximum.
            // This is needed, because the attribute is stateful and will convert from a string like
            // "100m" to the decimal value 100.
            //
            // Validate a randomly selected number.
            attribute.IsValid(3);

            _max = Convert.ToString(Attribute.Maximum, CultureInfo.InvariantCulture)!;
            _min = Convert.ToString(Attribute.Minimum, CultureInfo.InvariantCulture)!;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            MergeAttribute(context.Attributes, "max", _max);
            MergeAttribute(context.Attributes, "min", _min);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Minimum,
                Attribute.Maximum);
        }
    }
}
