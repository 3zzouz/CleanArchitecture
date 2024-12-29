using Microsoft.EntityFrameworkCore;
using Core.Domain.Models;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            string GenreJSon = System.IO.File.ReadAllText("genre.json");
            List<Genre>? genres = System.Text.Json.JsonSerializer.Deserialize<List<Genre>>(GenreJSon);
            //Seed to categorie
            foreach (Genre c in genres)
                model.Entity<Genre>()
                    .HasData(c);
            model.Entity<Customer>().HasMany(m => m.Movies).WithMany(c => c.customers);
            model.Entity<Customer>().HasMany(m => m.PreferredMovies);
        }

        public DbSet<Movie>? movies { get; set; }
        public DbSet<Genre> genres { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<MembershipType> membershipTypes { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }
    }
}