using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Utils
{
    /// <summary>
    /// Custom validation attribute to ensure email format is valid
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns>Validation Success OR Error message</returns>
    public class ValidEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Email is required");
            }
            else
            {
                var email = value.ToString();
                var atQuantity = email.Count(x => x == '@');

                if (email.Contains("@") && email.Contains(".") && atQuantity == 1)
                {
                    var emailParts = email.Split('@');
                    var emailDomain = emailParts[1].Split('.');

                    if ((emailDomain[0] == "gmail" || emailDomain[0] == "outlook" || emailDomain[0] == "yahoo") &&
                        (emailDomain[1] == "ca" || emailDomain[1] == "com" || emailDomain[1] == "in"))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Email domain should be: 'gmail', 'outlook', 'yahoo' and extensions: 'ca', 'com', 'in'.");
                    }

                }
                else
                {
                    return new ValidationResult("Email must contain '@' (only one) and at least one '.' (dot)");
                }
            }
        }
    }
}
