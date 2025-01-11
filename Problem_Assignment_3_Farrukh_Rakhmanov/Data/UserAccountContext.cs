using Final_Project_Farrukh_Rakhmanov.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Farrukh_Rakhmanov.Data
{
    public class UserAccountContext : DbContext
    {
        public UserAccountContext(DbContextOptions<UserAccountContext> options) : base(options)
        {
        }

        /// <summary>
        /// Represents Users table in the DB
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Represents User ProfilePicture table in DB
        /// </summary>
        public DbSet<ProfilePicture> ProfilePictures { get; set; }

        // DbSet properties represent database tables
        /// <summary>
        /// Represents the Books table in the database
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Represents the Genres table in the database
        /// </summary>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// This method is called when the model for the context is being created.
        /// It's used to configure the model and seed initial data.
        /// </summary>
        /// <param name="modelBuilder">Provides an API for configuring the model</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure inheritance using TPH
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Member>("Member");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "Admin@123",
                    ConfirmPassword = "Admin@123",
                    Name = "John",
                    Age = 28,
                    DateOfBirth = new DateTime(1996, 3, 26),
                    Role = "Admin"
                }              
            );

            modelBuilder.Entity<Member>().HasData(
               new Member
               {
                   Id = 2,
                   Username = "member",
                   Password = "Member@123",
                   ConfirmPassword = "Member@123",
                   Name = "Alex",
                   PhoneNumber = "123-456-7890",
                   Email = "john@gmail.com",
                   Age = 33,
                   DateOfBirth = new DateTime(1991, 7, 12),
                   Role = "Member"
               }
           );

            // Configuring the composite key in the join entity
            modelBuilder.Entity<MemberBook>()
                .HasKey(mb => new { mb.Id, mb.BookId });

            // Configuring the relationships
            modelBuilder.Entity<MemberBook>()
                .HasOne(mb => mb.Member)
                .WithMany(m => m.MemberBooks)
                .HasForeignKey(mb => mb.Id);

            modelBuilder.Entity<MemberBook>()
                .HasOne(mb => mb.Book)
                .WithMany(b => b.MemberBooks)
                .HasForeignKey(mb => mb.BookId);

            // Seeding data for the join table
            modelBuilder.Entity<MemberBook>().HasData(
                new MemberBook { Id = 2, BookId = 1 }, 
                new MemberBook { Id = 2, BookId = 2 }, 
                new MemberBook { Id = 2, BookId = 3 }  
            );

            // Seed initial data for Books table
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "The Great Gatsby",
                    Author = "Fitzgerald F. Scott",
                    GenreId = "1",  
                    Year = 1925,
                    IsAvailable = true,
                    Quantity = 5,           
                    AvailableQuantity = 4   
                },
                new Book
                {
                    BookId = 2,
                    Title = "Sapiens",
                    Author = "Yuval Noah Harari",
                    GenreId = "2",  
                    Year = 2011,
                    IsAvailable = true,
                    Quantity = 5,
                    AvailableQuantity = 5
                },
                new Book
                {
                    BookId = 3,
                    Title = "Dune",
                    Author = "Frank Herbert",
                    GenreId = "1",  
                    Year = 1965,
                    IsAvailable = true,
                    Quantity = 5,
                    AvailableQuantity = 5   
                },
                new Book
                {
                    BookId = 4,
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    GenreId = "3",  
                    Year = 1937,
                    IsAvailable = true,
                    Quantity = 5,
                    AvailableQuantity = 5
                },
                new Book
                {
                    BookId = 5,
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    GenreId = "1",  
                    Year = 1951,
                    IsAvailable = true,
                    Quantity = 5,
                    AvailableQuantity = 5
                }
            );

            // Seed initial data for Genres table
            modelBuilder.Entity<Genre>().HasData(
                new Genre
                {
                    GenreId = "1",
                    Name = "Fiction"
                },
                new Genre
                {
                    GenreId = "2",
                    Name = "Non-Fiction"
                },
                new Genre
                {
                    GenreId = "3",
                    Name = "Fantasy"
                },
                new Genre
                {
                    GenreId = "4",
                    Name = "Mystery"
                },
                new Genre
                {
                    GenreId = "5",
                    Name = "Science Fiction"
                }
            );
        }
    }
}
