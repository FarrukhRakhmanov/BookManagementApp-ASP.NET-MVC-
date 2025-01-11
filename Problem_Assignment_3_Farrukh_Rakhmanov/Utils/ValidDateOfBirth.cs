using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Utils
{
    public class ValidDateOfBirth : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is string dateOfBirth)
            {
                if (dateOfBirth.Length == 10)
                {
                    if (int.TryParse(dateOfBirth.Substring(0, 4), out int year) &&
                        int.TryParse(dateOfBirth.Substring(5, 2), out int month) &&
                        int.TryParse(dateOfBirth.Substring(8, 2), out int day))
                    {
                        if (year >= 1924 || year <= 2024)
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
                return new ValidationResult("Date of Birth must be in the format YYYY-MM-DD.");
            }
            return new ValidationResult("Date of Birth is required.");
        }
    }
}
