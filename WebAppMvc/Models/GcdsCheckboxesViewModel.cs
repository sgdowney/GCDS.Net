using System.ComponentModel.DataAnnotations;

namespace WebAppMvc.Models
{
    public class GcdsCheckboxesViewModel
    {
        [Required]
        [Display(Name = "FavouriteSeason")]
        public List<string>? FavouriteSeasons { get; set; }

        [Required]
        [Display(Name = "LeastFavouriteSeason")]
        public List<string>? LeastFavouriteSeasons { get; set; }

        [Required]
        [Display(Name = "FavouriteSeasonPreselected")]
        public List<string>? FavouriteSeasonsPreselected { get; set; }

        [Required]
        [Display(Name = "LeastFavouriteSeasonPreselected")]
        public List<string>? LeastFavouriteSeasonsPreselected { get; set; }
    }
}
