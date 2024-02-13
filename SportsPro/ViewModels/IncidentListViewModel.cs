/*
 * Quinton Nelson
 * 02/08/2024
 * This class defines a model of data that is needed by the 'List' view of the Incident manager
 */

using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentListViewModel
    {
        public List<Incident> Incidents {  get; set; }
        public string IncidentFilter { get; set; }

        //Also include lists of Customers, Products, and Technicians that the View will need
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<Technician> Technicianes { get; set; }
    }
}
