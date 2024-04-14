/*
 * Quinton Nelson
 * 4/13/2024
 * This attribute class is used to validate that the email address entered by the user is not already in use by another customer.
 */

using SportsPro.Data.DataLayer;
using SportsPro.Data.DataLayer.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models.Attributes
{
    public class EmailDuplicateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Using a service locator pattern to resolve dependencies
            var unitOfWork = validationContext.GetService<SportsProUnitOfWork>();

            if (unitOfWork == null)
            {
                return new ValidationResult("Database connection error.");
            }

            bool exists = false;

            if (value != null && value is string email)
            {
                // Use repository to check for existing email
                exists = unitOfWork.Customers.List(new QueryOptions<Customer>
                {
                    Where = c => c.Email == email
                }).Any();
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
