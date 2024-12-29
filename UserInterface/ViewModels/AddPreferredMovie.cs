using System.ComponentModel.DataAnnotations;

namespace UserInterface.ViewModels
{
    public class AddPreferredMovieViewModel
    {
        public int CustomerId { get; set; }
        [Display(Name = "Movie Name")] public int MovieId { get; set; }
    }
}