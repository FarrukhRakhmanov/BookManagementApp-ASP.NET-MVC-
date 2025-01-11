using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Models
{
    /// <summary>
    /// Represents a book genre/category in the library system
    /// Used for organizing and classifying books
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Unique identifier for the genre
        /// Nullable string to allow for custom genre codes
        /// </summary>
        public string? GenreId { get; set; }

        /// <summary>
        /// Name of the genre (e.g., "Fiction", "Science", "History")
        /// Must be between 2-100 characters
        /// Used for display and organization purposes
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Genre must be between 2 and 50 characters.")]
        public string Name { get; set; }

        // Note: This class could be extended with additional properties such as:
        // - Description
        // - ParentGenre (for hierarchical categorization)
        // - Shelf/Location information
        // - Age rating/restrictions
    }
}
