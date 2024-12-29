using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models;

public class Feedback
{
    public int id { get; set; }
    public string avis { get; set; }

    [Range(minimum: 1, maximum: 5, ErrorMessage = "La note doit être entre 1 et 5")]
    public int note { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
}