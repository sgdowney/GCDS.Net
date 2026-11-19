using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class GcdsDateInputViewModel
    {
        [Display(Name = "Full date")]
        [Required]
        public string? FullDateValue { get; set; }

        [Required]
        [Display(Name = "Full date prefill")]
        public required string FullDateValuePreFilled { get; set; }

        [Required]
        [Display(Name = "Year and month")]
        public string? YearMonthValue { get; set; }

        [Required]
        [Display(Name = "Year and month prefill")]
        public required string YearMonthValuePreFilled { get; set; }

        [Required]
        [Display(Name = "Full ISO date")]
        public string? FullDateIsoValue { get; set; }

        [Required]
        [Display(Name = "Full ISO date prefill")]
        public required string FullDateIsoValuePreFilled { get; set; }
    }
}
