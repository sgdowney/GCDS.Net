using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class GcdsSelectViewModel
    {
        [Required]
        [Display(Name = "Province of Residence")]
        public string? ProvinceOfResidence { get; set; }

        [Required]
        [Display(Name = "Province of Work")]
        public string? ProvinceOfWork { get; set; }

    }
}
