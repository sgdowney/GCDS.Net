using GoC.Components.DataAnnotations.Attributes;

using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(9)]
        [SIN]
        [Display(  Name = "Social Insurance Number")]
        public string? Sin { get; set; }

        [Required]
        [Display(Name = "Access Code")]
        [MaxLength(4)]
        [RegularExpression("[0-9]{4}")]
        public string? AccessCode { get; set; }

        [Required]
        [Display(Name = "Province of Residence")]
        public string? ProvinceId { get; set; }

        public required string HiddenField { get; set; }

        public string? AnotherHiddenField { get; set; }

        public string? ReadOnlyField { get; set; } = "Can't change me";

        [Required]
        public int RequiredField { get; set; } = 123456789;

        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        [Display(Name = "Address Type")]
        public string? EmailAddressType { get; set; }
    }
}
