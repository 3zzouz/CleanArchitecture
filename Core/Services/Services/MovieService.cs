using Core.Domain.IRepositories;
using Core.Domain.Models;
using Core.Services.Service_Contracts;

namespace Core.Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _filmRepository;

        public MovieService(IMovieRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public IEnumerable<Movie> GetAll()
        {
            return  _filmRepository.GetAll();
        }


        public IEnumerable<Movie> GetAllOrdered()
        {
            return  _filmRepository.GetAllOrdered();
        }

        public  IEnumerable<Movie> GetByGenreId(int genreId)
        {
            return  _filmRepository.GetByGenreId(genreId);
        }
    }
}
