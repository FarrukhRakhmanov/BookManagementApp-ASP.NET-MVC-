using System.ComponentModel.DataAnnotations;

namespace Final_Project_Farrukh_Rakhmanov.Utils
{
    /// <summary>
    /// Custom validation attribute to ensure phone number format is XXXXXXXXXX
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns>Validation Success OR Error message</returns>
    public class ValidPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Phone number is required");
            }
            else
            {
                var phoneNumber = value.ToString();
                if (phoneNumber.Length == 10)
                {
                    if (phoneNumber.StartsWith("0"))
                    {
                        return new ValidationResult("Phone number cannot start with  zero.");
                    }

                    for (int i = 0; i < phoneNumber.Length; i++)
                    {
                        if (!char.IsDigit(phoneNumber[i]))
                        {
                            return new ValidationResult("Phone number must be numeric");
                        }
                    }
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"{phoneNumber} length must be 10 digits");
                }
            }
        }
    }
}
