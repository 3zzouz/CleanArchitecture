using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Models;
using Infrastructure;

namespace UserInterface.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Feedback
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.feedbacks.Include(f => f.Customer).Include(f => f.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Feedback/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.feedbacks
                .Include(f => f.Customer)
                .Include(f => f.Movie)
                .FirstOrDefaultAsync(m => m.id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedback/Create
        public IActionResult Create(int customerId, int movieId)
        {
            ViewData["CustomerId"] = new SelectList(_context.customers, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.movies, "Id", "name");
            Feedback feedback = new Feedback()
            {
                CustomerId = customerId,
                MovieId = movieId
            };
            return View(feedback);
        }

        // POST: Feedback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,avis,note,CustomerId,MovieId")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                // Add debugging statements
                Console.WriteLine("Feedback object state before saving:");
                Console.WriteLine("ID: " + feedback.id);
                Console.WriteLine("Customer ID: " + feedback.CustomerId);
                Console.WriteLine("Movie ID: " + feedback.MovieId);

                // Check if CustomerId exists
                var customerExists = await _context.customers.AnyAsync(c => c.Id == feedback.CustomerId);
                if (!customerExists)
                {
                    ModelState.AddModelError("CustomerId", "Invalid Customer ID");
                    return View(feedback);
                }

                // Check if MovieId exists
                var movieExists = await _context.movies.AnyAsync(m => m.Id == feedback.MovieId);
                if (!movieExists)
                {
                    ModelState.AddModelError("MovieId", "Invalid Movie ID");
                    return View(feedback);
                }

                try
                {
                    _context.Add(feedback);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Customer",
                        new { id = feedback.CustomerId });
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Error saving feedback: " + ex.Message);
                    ModelState.AddModelError("",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            return View(feedback);
        }

        // GET: Feedback/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_context.customers, "Id", "Name", feedback.CustomerId);
            ViewData["MovieId"] = new SelectList(_context.movies, "Id", "name", feedback.MovieId);
            return View(feedback);
        }

        // POST: Feedback/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,avis,note,CustomerId,MovieId")] Feedback feedback)
        {
            if (id != feedback.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Customer",
                    new { id = feedback.CustomerId });
            }

            ViewData["CustomerId"] = new SelectList(_context.customers, "Id", "Name", feedback.CustomerId);
            ViewData["MovieId"] = new SelectList(_context.movies, "Id", "name", feedback.MovieId);
            return View(feedback);
        }

        // GET: Feedback/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.feedbacks
                .Include(f => f.Customer)
                .Include(f => f.Movie)
                .FirstOrDefaultAsync(m => m.id == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedback = await _context.feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.feedbacks.Remove(feedback);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Customer",
                new { id = feedback.CustomerId });
        }

        private bool FeedbackExists(int id)
        {
            return _context.feedbacks.Any(e => e.id == id);
        }
    }
}