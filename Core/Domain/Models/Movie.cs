using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required] public string name { get; set; }
        [Required] public DateTime? addedDate { get; set; }
        public byte[]? photo { get; set; }
        public Genre? genre { get; set; }
        public int? genreId { get; set; }
        public ICollection<Customer>? customers { get; set; }

        public ICollection<Feedback>? Feedbacks { get; set; }
        public float? rating { get; set; }
    }
}