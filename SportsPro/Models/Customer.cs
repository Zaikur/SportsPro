/* 
 * Jason Nelson
 * 04/11/2024
 * Added an ICollection for Registration
 */

using System.ComponentModel.DataAnnotations;
using SportsPro.Models.Attributes;

namespace SportsPro.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(51, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(51, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(51, MinimumLength = 1)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(51, MinimumLength = 1)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [StringLength(51, MinimumLength = 1)]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required.")]
        [StringLength(21, MinimumLength = 1)]
        public string PostalCode { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone format.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid phone format. Use ###-###-####.")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [EmailDuplicate]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please select a country.")]
        public string CountryID { get; set; } = string.Empty;   // foreign key property

        public Country? Country { get; set; }          // navigation property

        public string FullName => FirstName + " " + LastName;   // read-only property

        //Gets or sets the collection of registrations associated with the customer.
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
