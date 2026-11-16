using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class GcdsRadiosViewModel
    {
        [Required]
        [Display(Name = "FavouriteSeason")]
        public string? FavouriteSeason { get; set; }

        [Required]
        [Display(Name = "LeastFavouriteSeason")]
        public string? LeastFavouriteSeason { get; set; }

        [Required]
        [Display(Name = "FavouriteSeasonPreselected")]
        public string? FavouriteSeasonPreselected { get; set; }

        [Required]
        [Display(Name = "LeastFavouriteSeasonPreselected")]
        public string? LeastFavouriteSeasonPreselected { get; set; }
    }
}
