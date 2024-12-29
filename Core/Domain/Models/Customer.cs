using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required] public string? Name { get; set; }
        public int? MembershipTypeId { get; set; }
        public MembershipType? MembershipType { get; set; }
        public ICollection<Movie>? Movies { get; set; }
        public ICollection<Movie>? PreferredMovies { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }

    public class CustomerDetailsViewModel
    {
        public Customer Customer { get; set; } = new Customer();
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}