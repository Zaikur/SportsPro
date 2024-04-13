/* 
 * Jason Nelson
 * 04/11/2024
 * Added an ICollection for Registration
 */

/*
 * Ayden Hofts
 * 04/13/2024
 * Added improved validation for the edit customer page
 */

using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Required.")]
        [StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters.", MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required.")]
        [StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters.", MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required.")]
        [StringLength(50, ErrorMessage = "Address must be between 1 and 50 characters.", MinimumLength = 1)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required.")]
        [StringLength(50, ErrorMessage = "City must be between 1 and 50 characters.", MinimumLength = 1)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required.")]
        [StringLength(50, ErrorMessage = "State must be between 1 and 50 characters.", MinimumLength = 1)]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required.")]
        [StringLength(20, ErrorMessage = "Postal code must be between 1 and 21 characters.", MinimumLength = 1)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP code format.")]
        public string PostalCode { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone format.")]
        [RegularExpression(@"^\(?\d{3}\)?[-\s]?\d{3}-\d{4}$", ErrorMessage = "Phone number must be in the (999) 999-9999 format.")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        [StringLength(50, ErrorMessage = "Email must be between 1 and 50 characters.", MinimumLength = 1)]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Please select a country.")]
        public string CountryID { get; set; } = string.Empty;   // foreign key property

        public Country? Country { get; set; }          // navigation property

        public string FullName => FirstName + " " + LastName;   // read-only property

        //Gets or sets the collection of registrations associated with the customer.
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
