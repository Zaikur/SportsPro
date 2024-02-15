/*
 * Ayden Hofts
 * 01/22/2024
 * This is the controller for the incident section of the website
 * 
 * Quinton Nelson
 * 2/14/2024
 * Updated Add, Edit, and List methods to utilize View Models
 */



using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using SportsPro.ViewModels;


namespace SportsPro.Controllers

{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        //Include lists of needed objects
        private List<Product> products;
        private List<Technician> technicians;
        private List<Customer> customers;

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;

            //Populate the lists with data from the database to send to the views
            products = context.Products.ToList();
            technicians = context.Technicians.ToList();
            customers = context.Customers.ToList();
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("Incidents")]
        public IActionResult List()
        {
            // Fetch the incidents from the database
            var incidents = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .OrderBy(i => i.IncidentID).ToList();

            // Create an instance of the ViewModel
            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents,
                
                //Set up filtering structure for later use
                IncidentFilter = "All",

                // Populated lists for use in filtering
                Customers = context.Customers.ToList(), 
                Products = context.Products.ToList(),
                Technicians = context.Technicians.ToList()
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Display the Add/Edit Incident page with blank fields
            var viewModel = new IncidentAddEditViewModel
            {
                OperationType = "Add",
                Customers = customers,
                Technicians = technicians,
                Products = products,
                CurrentIncident = new Incident()
            };


            return View("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var incident = context.Incidents
                .Include(c => c.Customer)
                .Include(c => c.Technician)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.IncidentID == id);

            //Add the lists to the ViewModel and send to the View
            var viewModel = new IncidentAddEditViewModel
            {
                OperationType = "Edit",
                Customers = customers,
                Technicians = technicians,
                Products = products,
                CurrentIncident = incident
            };


            return View("Edit", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(IncidentAddEditViewModel viewModel)
        {
            // Handle the form submission for adding/editing incidents
            if (ModelState.IsValid)
            {
                //Get current incident from the viewModel
                if (viewModel.CurrentIncident.IncidentID == 0)
                {
                    context.Incidents.Add(viewModel.CurrentIncident);
                }
                else
                {
                    context.Incidents.Update(viewModel.CurrentIncident);
                }

                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                viewModel.OperationType = viewModel.CurrentIncident.IncidentID == 0 ? "Add" : "Edit";

                // Repopulate lists if validation fails
                viewModel.Customers = customers; 
                viewModel.Technicians = technicians;
                viewModel.Products = products;

                return View("Edit", viewModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Display the Delete Incident page that confirms the deletion
            var incident = context.Incidents.Find(id);
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            // Handle the form submission for deleting incidents
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List", "Incident");
        }
    }
}
