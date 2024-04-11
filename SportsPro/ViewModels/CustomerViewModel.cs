/*
 * Jason Nelson
 * 04/11/2024
 * This class defines a model of data that is needed by the 'GetCustomer' and 'ListByCustomer' views of the Registrations manager
 */

using SportsPro.Models;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.ViewModels
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Please select a customer.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a customer.")]
        public int SelectedCustomerId { get; set; }
        public string SelectedCustomerName { get; set; } = null!;
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Registration> Registrations { get; set; } = new List<Registration>();
    }
}