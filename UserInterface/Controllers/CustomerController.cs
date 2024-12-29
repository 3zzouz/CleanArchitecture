using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Models;
using Infrastructure;
using UserInterface.ViewModels;

namespace UserInterface.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.customers.Include(c => c.MembershipType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.MembershipType)
                .Include(c => c.PreferredMovies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var movies = await _context.movies
                .Include(m => m.genre)
                .Include(m => m.Feedbacks)
                .ToListAsync();
            var viewModel = new CustomerDetailsViewModel
            {
                Customer = customer,
                Movies = movies
            };

            return View(viewModel);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            ViewData["MembershipTypeId"] = new SelectList(_context.membershipTypes, "Id", "Id");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MembershipTypeId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MembershipTypeId"] =
                new SelectList(_context.membershipTypes, "Id", "Id", customer.MembershipTypeId);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewData["MembershipTypeId"] =
                new SelectList(_context.membershipTypes, "Id", "Id", customer.MembershipTypeId);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MembershipTypeId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["MembershipTypeId"] =
                new SelectList(_context.membershipTypes, "Id", "Id", customer.MembershipTypeId);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.MembershipType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.customers.FindAsync(id);
            if (customer != null)
            {
                _context.customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // public async Task<IActionResult> AddPreferredMovie(int? customerId)
        // {
        //     if (customerId == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var customer = await _context.customers
        //         .Include(c => c.PreferredMovies)
        //         .FirstOrDefaultAsync(m => m.Id == customerId);
        //     if (customer == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     ViewData["MovieId"] = new SelectList(_context.movies, "id", "name");
        //     return View(new AddPreferredMovieViewModel() { CustomerId = customerId.Value });
        // }

        // POST: Customer/AddPreferredMovie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPreferredMovie(int customerId, int movieId)
        {
            var customer = await _context.customers
                .Include(c => c.PreferredMovies)
                .FirstOrDefaultAsync(m => m.Id == customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var movie = await _context.movies.FindAsync(movieId);
            if (movie == null)
            {
                return NotFound();
            }

            customer.PreferredMovies.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = customerId });
        }

        // POST: Customer/RemovePreferredMovie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePreferredMovie(int customerId, int movieId)
        {
            var customer = await _context.customers
                .Include(c => c.PreferredMovies)
                .FirstOrDefaultAsync(m => m.Id == customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var movie = customer.PreferredMovies.FirstOrDefault(m => m.Id == movieId);
            if (movie == null)
            {
                return NotFound();
            }

            customer.PreferredMovies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = customerId });
        }

        private bool CustomerExists(int id)
        {
            return _context.customers.Any(e => e.Id == id);
        }
    }
}