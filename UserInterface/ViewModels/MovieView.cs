using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Models;

namespace UserInterface.ViewModels
{
    [NotMapped]
    public class MovieView
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public DateTime? addedDate { get; set; }
        public string? photo { get; set; }
        public int? genre_id { get; set; }
        public Genre? genre { get; set; }
        public ICollection<Customer>? customers { get; set; }
        public double? rating { get; set; }
    }
    
    public class GroupedMovieViewModel
    {
        public string GenreName { get; set; }
        public List<MovieView> Movies { get; set; }
    }
}
