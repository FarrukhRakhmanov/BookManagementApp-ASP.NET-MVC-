using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Utils
{
    public class ValidPassword : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Password is required");
            }
            var password = value.ToString();

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required");
            }
            if (password.Length < 8)
            {
                return new ValidationResult("Password must be at least 8 characters long");
            }

            if (!password.Any(char.IsUpper))
            {
                return new ValidationResult("Password must contain at least one uppercase letter");
            }

            if (!password.Any(char.IsDigit))
            {
                return new ValidationResult("Password must contain at least one number");
            }

            if (!password.Any(ch => !char.IsPunctuation(ch)))
            {
                return new ValidationResult("Password must contain at least one special character");
            }

            if (password.Any(char.IsWhiteSpace))
            {
                return new ValidationResult("Password must not contain any whitespace characters");
            }

            return ValidationResult.Success;
        }

    }
}
