/*
 * Quinton Nelson
 * 4/13/2024
 * This attribute class is used to validate that the email address entered by the user is not already in use by another customer.
 * 
 * Quinton Nelson
 * 4/14/2024
 * Modified this attribute to use the IRepository<Customer> service to access the database.
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
            // Get the IRepository<Customer> using the service provider
            var customerRepository = validationContext.GetService<IRepository<Customer>>();

            if (customerRepository == null)
            {
                return new ValidationResult("Unable to access customer repository.");
            }

            bool exists = false;

            if (value != null && value is string email)
            {
                // Use the customer repository to check for existing email
                exists = customerRepository.List(new QueryOptions<Customer>
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
