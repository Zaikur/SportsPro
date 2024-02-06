/*
 * Ayden Hofts
 * 01/22/2024
 * This is the controller for the incident section of the website
 */



using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;


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
            var incidents = context.Incidents
                .OrderBy(i => i.IncidentID).ToList();
            return View(incidents);
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Display the Add/Edit Incident page with blank fields
            ViewBag.Action = "Add";
            ViewBag.Customers = customers;
            ViewBag.Technicians = technicians;
            ViewBag.Products = products;


            return View("Edit", new Incident());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Display the Add/Edit Incident page with the data for the selected incident
            ViewBag.Action = "Edit";

            var incident = context.Incidents
                .Include(c => c.Customer)
                .Include(c => c.Technician)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.IncidentID == id);

            //Add the lists to the ViewBag for use in the View on the SelectList methods
            ViewBag.Customers = customers;
            ViewBag.Technicians = technicians;
            ViewBag.Products = products;


            return View("Edit", incident);
        }

        [HttpPost]
        public IActionResult Edit(Incident incident)
        {
            // Handle the form submission for adding/editing incidents
            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                {
                    context.Incidents.Add(incident);
                }
                else
                {
                    context.Update(incident);
                }

                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Customers = customers;
                ViewBag.Technicians = technicians;
                ViewBag.Products = products;

                return View("Edit");
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
