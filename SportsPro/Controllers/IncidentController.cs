/*
 * Ayden Hofts
 * 01/22/2024
 * This is the controller for the incident section of the website
 * 
 * Quinton Nelson
 * 2/14/2024
 * Updated Add, Edit, and List methods to utilize View Models
 * 
 * Quinton Nelson
 * 3/10/2024
 * Add GetTechnician method to retrieve a list of technicians from the database
 * Add ListByTechnician method to filter incidents by technician
 * Modify Edit method to account for different operation types (From the technician view)
 * 
 * Jason Nelson
 * 03/15/2024
 * Added methods to filter incidents based on if they are unassigned or open
 */



using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using SportsPro.ViewModels;
using SportsPro.Data.DataLayer.Repositories;
using SportsPro.Data.DataLayer;

namespace SportsPro.Controllers

{
    public class IncidentController : Controller
    {
        private SportsProUnitOfWork data { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            data = new SportsProUnitOfWork(ctx);
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("Incidents")]
        public IActionResult List()
        {
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                OrderBy = inc => inc.IncidentID
            };

            var incidents = data.Incidents.List(options);
            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents.ToList(),
                IncidentFilter = "All",
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList()
            };

            return View(viewModel);
        }

        [HttpGet("Incidents/Unassigned")]
        public IActionResult ListUnassigned()
        {
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                Where = inc => inc.TechnicianID == -1,
                OrderBy = inc => inc.IncidentID
            };
            var incidents = data.Incidents.List(options);
            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents.ToList(),
                IncidentFilter = "Unassigned",
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList()
            };

            return View("List", viewModel); // Reuse the List view with the filtered incidents
        }

        [HttpGet("Incidents/Open")]
        public IActionResult ListOpenIncidents()
        {
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                OrderBy = inc => inc.IncidentID
            };
            var incidents = data.Incidents.List(options);
            var viewModel = new IncidentListViewModel
            {
                Incidents = incidents.ToList(),
                IncidentFilter = "Open",
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList()
            };

            return View("List", viewModel); // Reuse the List view with the filtered incidents
        }

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new IncidentAddEditViewModel
            {
                OperationType = "Add",
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                CurrentIncident = new Incident()
            };

            return View("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                Where = inc => inc.IncidentID == id
            };

            var incident = data.Incidents.Get(options);

            var viewModel = new IncidentAddEditViewModel
            {
                OperationType = "Edit",
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                CurrentIncident = incident
            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(IncidentAddEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.CurrentIncident.IncidentID == 0)
                {
                    data.Incidents.Insert(viewModel.CurrentIncident);
                }
                else
                {
                    data.Incidents.Update(viewModel.CurrentIncident);
                }
                data.Save();
                return RedirectToAction("List");
            }

            viewModel = new IncidentAddEditViewModel
            {
                OperationType = "Add",
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                CurrentIncident = new Incident()
            };
            return View("Edit", viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Display the Delete Incident page that confirms the deletion
            var incident = data.Incidents.Get(id);
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            // Handle the form submission for deleting incidents
            data.Incidents.Delete(incident);
            data.Save();
            return RedirectToAction("List", "Incident");
        }

        //Get a list of technicians from the database and return them to the view
        [Route("TechIncident")]
        [HttpGet]
        public IActionResult GetTechnician()
        {
            var model = new TechnicianViewModel()
            {
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList()
            };

            return View(model);
        }

        //Get the selected technician from the view and redirect to the ListByTechnician action
        [HttpPost]
        [Route("TechIncident")]
        public IActionResult GetTechnician(TechnicianViewModel model)
        {
            // Attempt to retrieve the technician from the database
            var technician = data.Technicians.Get(model.SelectedTechnicianId);

            // Save the technician in session state if it exists
            if (technician != null)
            {
                HttpContext.Session.SetInt32("SelectedTechnicianId", model.SelectedTechnicianId);
                HttpContext.Session.SetString("TechnicianName", data.Technicians.Get(model.SelectedTechnicianId).Name);
            }


            // Redirect to the ListByTechnician action, passing the selected technician ID.
            return RedirectToAction("ListByTechnician", new { id = model.SelectedTechnicianId });
        }

        // Filter incidents by technician
        [Route("techincident/list/{id?}")]
        [HttpGet]
        public IActionResult ListByTechnician(int? id)
        {
            // Get the selected technician from session state
            id = HttpContext.Session.GetInt32("SelectedTechnicianId") ?? 0;

            // If no technician is selected, redirect to the GetTechnician action
            if (id == 0)
            {
                return RedirectToAction("GetTechnician");
            }
            ViewBag.TechnicianName = HttpContext.Session.GetString("TechnicianName");

            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product, Technician",
                Where = inc => inc.TechnicianID == id
            };

            var incident = data.Incidents.Get(options);

            var viewModel = new IncidentListViewModel
            {
                Customers = data.Customers.List(new QueryOptions<Customer>()).ToList(),
                Technicians = data.Technicians.List(new QueryOptions<Technician>()).ToList(),
                Products = data.Products.List(new QueryOptions<Product>()).ToList(),
                Incidents = data.Incidents.List(options).ToList()
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }
    }
}
