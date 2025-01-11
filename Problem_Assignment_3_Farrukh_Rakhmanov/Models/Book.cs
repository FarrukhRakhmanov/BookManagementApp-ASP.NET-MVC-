using Final_Project_Farrukh_Rakhmanov.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace Final_Project_Farrukh_Rakhmanov.Models
{

    /// <summary>
    /// Represents a book in the library system with validation rules
    /// and inventory management functionality
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Unique identifier for the book
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Title of the book
        /// Requires 2-100 characters
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        public string? Title { get; set; }

        /// <summary>
        /// Author of the book
        /// Requires 2-50 characters
        /// </summary>
        [Required(ErrorMessage = "Author is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Author must be between 2 and 50 characters.")]
        public string? Author { get; set; }

        /// <summary>
        /// Publication year of the book
        /// Validated using custom ValidYear attribute defined in Utilities.cs
        /// </summary>
        [ValidYear]
        public int Year { get; set; }

        /// <summary>
        /// Indicates if the book is available for checkout
        /// Automatically managed based on AvailableQuantity
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Total number of copies owned by the library
        /// Must be at least 1
        /// </summary>
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 9999, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Number of copies currently available for checkout
        /// Updated automatically when books are issued or returned
        /// </summary>
        public int AvailableQuantity { get; set; }

        /// <summary>
        /// Foreign key reference to the book's genre
        /// </summary>
        [Required(ErrorMessage = "Genre is required.")]
        public string GenreId { get; set; }

        /// <summary>
        /// Navigation property to the associated Genre
        /// Excluded from model validation
        /// </summary>
        [ValidateNever]
        public Genre? Genre { get; set; }

        // Navigation property
        public ICollection<MemberBook>? MemberBooks { get; set; }

        /// <summary>
        /// Validates and updates the book's availability status
        /// based on the current AvailableQuantity
        /// </summary>
        public void SetDefaults()
        {
            // If there are copies available but status is false, update to true
            if (AvailableQuantity > 0 && IsAvailable == false)
            {
                IsAvailable = true;
            }
            // If no copies are available but status is true, update to false
            if (AvailableQuantity == 0 && IsAvailable == true)
            {
                IsAvailable = false;
            }
        }

        /// <summary>
        /// Processes a book checkout:
        /// - Decrements available quantity
        /// - Updates availability status if no copies remain
        /// </summary>
        public void IssueBook()
        {
            AvailableQuantity--;
            if (AvailableQuantity == 0)
            {
                IsAvailable = false;
            }
        }

        /// <summary>
        /// Processes a book return:
        /// - Increments available quantity
        /// - Sets availability status to true
        /// </summary>
        public void ReturnBook()
        {
            AvailableQuantity++;
            IsAvailable = true;
        }

        public static List<Book> SearchBooks(List<Book> books, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return books;
            }
            searchTerm = searchTerm.ToLower();

            return books.
                Where(b =>
                   b.Title.ToLower().Contains(searchTerm)
                || b.Author.ToLower().Contains(searchTerm)
                || b.Genre.Name.ToLower().Contains(searchTerm)).ToList();
        }

        public static List<Book> FilterByGenre(List<Book> books, string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return books;
            }
            return books.Where(b => b.Genre.Name.ToLower() == genre.ToLower()).ToList();
        }

        public static List<Book> SortBooks(List<Book> books, string sortBy)
        {
            switch (sortBy)
            {
                case "title":
                    return books.OrderBy(b => b.Title).ToList();
                case "title_desc":
                    return books.OrderByDescending(b => b.Title).ToList();
                case "author":
                    return books.OrderBy(b => b.Author).ToList();
                case "author_desc":
                    return books.OrderByDescending(b => b.Author).ToList();
                case "genre":
                    return books.OrderBy(b => b.Genre.Name).ToList();
                case "genre_desc":
                    return books.OrderByDescending(b => b.Genre.Name).ToList();
                case "year":
                    return books.OrderBy(b => b.Year).ToList();
                case "year_desc":
                    return books.OrderByDescending(b => b.Year).ToList();
                case "quantity":
                    return books.OrderBy(b => b.Quantity).ToList();
                case "quantity_desc":
                    return books.OrderByDescending(b => b.Quantity).ToList();
                case "availableQuantity":
                    return books.OrderBy(b => b.AvailableQuantity).ToList();
                case "availableQuantity_desc":
                    return books.OrderByDescending(b => b.AvailableQuantity).ToList();
                case "isAvailable":
                    return books.OrderBy(b => b.IsAvailable).ToList();
                case "isAvailable_desc":
                    return books.OrderByDescending(b => b.IsAvailable).ToList();
                default:
                    return books.OrderBy(b => b.Title).ToList();
            }
        }
        public static List<string> GetGenres(List<Book> books)
        {
            return books.Select(b => b.Genre.Name).Distinct().ToList();
        }

        public static List<Book> FilterBy(List<Book> books, string filterBy)
        {
            switch (filterBy)
            {
                case "isAvailable":
                    return books.Where(b => b.IsAvailable == true).ToList();
                case "notAvailable":
                    return books.Where(b => b.IsAvailable == false).ToList();
                default:
                    return books.OrderBy(b => b.Title).ToList();
            }
        }

    }
}

