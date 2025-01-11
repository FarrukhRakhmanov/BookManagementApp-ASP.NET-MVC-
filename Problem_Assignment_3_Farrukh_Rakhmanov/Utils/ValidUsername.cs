using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Utils
{
    public class ValidUsername : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Username is required");
            }

            var username = value.ToString();

            if (string.IsNullOrEmpty(username))
            {
                return new ValidationResult("Username is required");
            }

            if (username.Length < 5 || username.Length > 20)
            {
                return new ValidationResult("Username must be between 5 and 20 characters long");
            }

            return ValidationResult.Success;
        }
    }
}

