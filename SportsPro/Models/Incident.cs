/*
 * Ayden Hofts
 * 01/22/2024
 * This is the model for the incident section of the website
 */


using System.ComponentModel.DataAnnotations;


namespace SportsPro.Models


{
    public class Incident
    {
		public int IncidentID { get; set; }
        
		[Required(ErrorMessage = "Please select a title.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a description.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the date opened.")]
        [DataType(DataType.Date)]
        public DateTime DateOpened { get; set; } = DateTime.Now;
        public DateTime? DateClosed { get; set; } = null;

        [Required(ErrorMessage = "Please select a Customer.")]
        public int CustomerID { get; set; }            // foreign key property
        //public Customer Customer { get; set; } = null!;       // navigation property

        public Customer? Customer { get; set; }

        [Required(ErrorMessage = "Please select a Product.")]
        public int ProductID { get; set; }                    // foreign key property
        //public Product Product { get; set; } = null!;         // navigation property

        public Product? Product { get; set; }

        [Required(ErrorMessage = "Please select a Technician.")]
        public int TechnicianID { get; set; }                 // foreign key property 
		//public Technician Technician { get; set; } = null!;   // navigation property

        public Technician? Technician { get; set; }

    }
}