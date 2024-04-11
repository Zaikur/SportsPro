/*
 * Jason Nelson
 * 04/11/2024
 * Entity clas that provides a many-to-many relationship between the Customer and Product entities
 */

using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
