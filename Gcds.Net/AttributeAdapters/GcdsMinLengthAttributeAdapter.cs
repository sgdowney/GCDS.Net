using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Gcds.Net.AttributeAdapters
{
    public class GcdsMinLengthAttributeAdapter : AttributeAdapterBase<MinLengthAttribute>
    {
        private readonly string _minLength; 

        public GcdsMinLengthAttributeAdapter(MinLengthAttribute attribute, IStringLocalizer? stringLocalizer): base(attribute, stringLocalizer) 
        {
            _minLength = Attribute.Length.ToString(CultureInfo.InvariantCulture);
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            MergeAttribute(context.Attributes, "minlength", _minLength);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName(), Attribute.Length);

        }
    }
}
