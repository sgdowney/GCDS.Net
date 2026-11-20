using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class GcdsTextareaViewModel
    {
        [Required]
        [Display(Name = "Description")]
        [StringLength(200, MinimumLength = 20)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Comments")]
        public string? Comments { get; set; }
    }
}
