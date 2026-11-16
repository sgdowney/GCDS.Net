using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class GcdsInputViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(16, MinimumLength = 8)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Age of Eligibility")]
        [Range(18, 65)]
        public int AgeOfEligibility { get; set; }

        [Required]
        [Display(Name = "PrefilledField")]
        public string? PrefilledField { get; set; }

    }
}
