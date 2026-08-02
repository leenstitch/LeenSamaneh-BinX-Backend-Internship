// this code defines a DbContext for an API project that manges models for a library system
using APIProject.Models;
using Microsoft.EntityFrameworkCore;

namespace APIProject.Data
{
    public class LibraryDbContext : DbContext
    {
        // ============= CONSTRUCTOR ==========
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {

        }


        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }


        // ============= SEED DATA ==========
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Authors and Books
            modelBuilder.Entity<Author>().HasData(

                new Author
                {
                    Id = 1,
                    Name = "J.K Rowling",
                    Nationality = "British"
                },


                new Author
                {
                    Id = 2,
                    Name = "Robert C. Martin",
                    Nationality = "American"
                }

            );


            modelBuilder.Entity<Book>().HasData(

                new Book
                {
                    Id = 1,
                    Title = "Harry Potter",
                    Description = "Amazing book",
                    Price = 25,
                    AuthorId = 1
                },


                new Book
                {
                    Id = 2,
                    Title = "Clean Code",
                    Description = "Great book",
                    Price = 40,
                    AuthorId = 1
                },


                new Book
                {
                    Id = 3,
                    Title = "The Hobbit",
                    Description = "Good book",
                    Price = 30,
                    AuthorId = 2
                }

            );
        }
    }
}