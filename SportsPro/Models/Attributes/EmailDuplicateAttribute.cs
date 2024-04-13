/*
 * Quinton Nelson
 * 4/13/2024
 * This attribute class is used to validate that the email address entered by the user is not already in use by another customer.
 */

using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models.Attributes
{
    public class EmailDuplicateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetService<IServiceProvider>();
            var context = serviceProvider?.GetRequiredService<SportsProContext>();

            if (context == null)
            {
                return new ValidationResult("Database connection error.");
            }

            bool exists = false;

            if (value != null)
            {
                exists = context.Customers.Any(c => c.Email == value.ToString());
            }
            

            if (exists)
            {
                return new ValidationResult("Email address is already in use.");
            } 
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
