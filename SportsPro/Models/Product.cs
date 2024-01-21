/* 
 * Jason Nelson
 * 01/19/2024
 * Product model to validate data
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter a product code.")]
        public string ProductCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a yearly price.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Yearly price must be greater than $0.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number for the yearly price.")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal YearlyPrice { get; set; }

        [Required(ErrorMessage = "Please enter a date.")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
    }
}
