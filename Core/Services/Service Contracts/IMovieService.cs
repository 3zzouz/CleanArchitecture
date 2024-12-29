using Core.Domain.Models;

namespace Core.Services.Service_Contracts
{
    public interface IMovieService
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> GetByGenreId(int genreId);
        IEnumerable<Movie> GetAllOrdered();
    }
}
