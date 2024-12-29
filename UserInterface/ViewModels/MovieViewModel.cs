using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Models;


namespace UserInterface.ViewModels
{
    [NotMapped]
    public class MovieViewModel
    {
        public int id { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public DateTime? addedDate { get; set; }
        public IFormFile? photo { get; set; }
        public int? genre_id { get; set; }
        public Genre? genre { get; set; }
        public ICollection<Customer>? customers { get; set; }
    }
}
