using Final_Project_Farrukh_Rakhmanov.Utils;
using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [ValidUsername]
        public string Username { get; set; }

        [ValidPassword]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Confirm Password must match Password.")]
        public string ConfirmPassword { get; set; }

        public ProfilePicture? ProfilePicture { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }


        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
