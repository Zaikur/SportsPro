/*
 * Quinton Nelson
 * 02/14/2024
 * This class defines a model of data that is needed by the 'Add/Edit' view of the Incident manager
 */

using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentAddEditViewModel
    {
        public Incident CurrentIncident { get; set; } = new Incident();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Technician> Technicians { get; set; } = new List<Technician>();

        //Add or Edit string
        public string OperationType { get; set; } = "Add";
    }
}
