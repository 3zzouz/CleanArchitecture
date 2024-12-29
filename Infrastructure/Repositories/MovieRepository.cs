using Core.Domain.IRepositories;
using Core.Domain.Models;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.movies.ToList();
        }

        public IEnumerable<Movie> GetByGenreId(int genreId)
        {
            return _context.movies
                .Where(f => f.genre.Id == genreId)
                .ToList();
        }

        public IEnumerable<Movie> GetAllOrdered()
        {
            return _context.movies
                .OrderBy(f => f.addedDate)
                .ToList();
        }
    }
}