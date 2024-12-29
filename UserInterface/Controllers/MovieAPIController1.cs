using Core.Domain.Models;
using Core.Services.Service_Contracts;
using Microsoft.AspNetCore.Mvc;
using Core.Services.Services;

namespace tp3.Controllers
{
    [ApiController]
    [Route("MovieAPIController1")]
    public class MovieAPIController1 : Controller
    {
        private readonly MovieService _movieService;
        public MovieAPIController1(IMovieService movieService)
        {
            _movieService = (MovieService) movieService;
        }
        [HttpGet("genre/id/{genreId}")]
        public  IEnumerable<Movie> GetByGenreId(int genreId)
        {
            return  _movieService.GetByGenreId(genreId);
        }
        [HttpGet("AllOrdered")]
        public IEnumerable<Movie> GetALLOrdered()
        {
            return _movieService.GetAllOrdered();
        }
        // GET: MovieAPIController1
        public ActionResult Index()
        {
            return View();
        }

        // GET: MovieAPIController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovieAPIController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovieAPIController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MovieAPIController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MovieAPIController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MovieAPIController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovieAPIController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
