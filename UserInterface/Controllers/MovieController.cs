using Core.Domain.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserInterface.ViewModels;

namespace UserInterface.Controllers
{
    public class MovieController : Controller

    {
        private ApplicationDbContext _dbContext;

        public MovieController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: MoviesControllercs
        public IFormFile ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName)
        {
            var stream = new MemoryStream(fileBytes); // Crée un MemoryStream à partir du tableau de bytes
            var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg" // Définissez le type MIME approprié pour votre image
            };
            return formFile;
        }

        public ActionResult Index()
        {
            var groupedMovies = _dbContext.movies.Include(m => m.genre).GroupBy(m => m.genre.GenreName);
            var viewModel = groupedMovies.Select(group => new GroupedMovieViewModel
            {
                GenreName = group.Key,
                Movies = group.Select(m => new MovieView
                {
                    id = m.Id,
                    name = m.name,
                    addedDate = m.addedDate,
                    photo = m.photo != null ? Convert.ToBase64String(m.photo) : null,
                    genre = m.genre,
                    rating = _dbContext.feedbacks.Where(f => f.MovieId == m.Id).Average(f => f.note)

                }).ToList()
            }).ToList();
            return View(viewModel);
        }

        // GET: MoviesControllercs/Details/5
        public ActionResult Details(int id)
        {
            var movie = _dbContext.movies.Include(m => m.genre).FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieView
            {
                id = movie.Id,
                name = movie.name,
                addedDate = movie.addedDate,
                photo = Convert.ToBase64String(movie.photo),
                genre = movie.genre,
                rating = _dbContext.feedbacks.Where(f => f.MovieId == movie.Id).Average(f => f.note)
            };
            return View(viewModel);
        }

        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_dbContext.genres, "Id", "GenreName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieViewModel movie)
        {
            if (ModelState.IsValid)
            {
                if (movie.photo != null && movie.photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        movie.photo.CopyTo(memoryStream);
                        var photoBytes = memoryStream.ToArray();

                        var movieEntity = new Movie()
                        {
                            name = movie.name,
                            addedDate = movie.addedDate,
                            photo = photoBytes,
                            genreId = movie.genre_id
                        };
                        _dbContext.movies.Add(movieEntity);
                        _dbContext.SaveChanges();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreId = new SelectList(_dbContext.genres, "Id", "GenreName", movie.genre_id);
            return View(movie);
        }

        public ActionResult Edit(int id)
        {
            var movie = _dbContext.movies.Include(m => m.genre).FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieViewModel
            {
                id = movie.Id,
                name = movie.name,
                addedDate = movie.addedDate,
                genre_id = movie.genreId,
                photo = ConvertByteArrayToIFormFile(movie.photo, movie.name),
            };

            ViewBag.GenreId = new SelectList(_dbContext.genres, "Id", "GenreName", movie.genreId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MovieViewModel movie)
        {
            if (id != movie.id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var movieEntity = _dbContext.movies.FirstOrDefault(m => m.Id == id);
                if (movieEntity == null)
                {
                    return NotFound();
                }

                movieEntity.name = movie.name;
                movieEntity.addedDate = movie.addedDate;
                movieEntity.genreId = movie.genre_id;

                if (movie.photo != null && movie.photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        movie.photo.CopyTo(memoryStream);
                        movieEntity.photo = memoryStream.ToArray();
                    }
                }

                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreId = new SelectList(_dbContext.genres, "Id", "GenreName", movie.genre_id);
            return View(movie);
        }

        public ActionResult Delete(int id)
        {
            var movie = _dbContext.movies.Include(m => m.genre).FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieView
            {
                id = movie.Id,
                name = movie.name,
                addedDate = movie.addedDate,
                photo = Convert.ToBase64String(movie.photo),
                genre = movie.genre
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var movie = _dbContext.movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _dbContext.movies.Remove(movie);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}