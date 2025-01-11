using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Utils
{
    /// <summary>
    /// Validates that the year is between 1000 and the current year
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns>Validation Success OR Error message</returns>
    public class ValidYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && int.TryParse(value.ToString(), out int year))
            {
                int currentYear = DateTime.Now.Year;
                if (year >= 1000 && year <= currentYear)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"Year must be between 1000 and {currentYear}.");
            }
            return new ValidationResult("Year is required.");
        }
    }
}
