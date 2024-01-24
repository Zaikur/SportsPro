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

        /*Expand the controller to include the lists - Quinton
         * 
         * 
         * public IncidentController(SportsProContext ctx) => context = ctx;
         */

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;

            //Populate the lists with data from the database to send to the views
            //Not having this and sending them to the view is why SelectList wasn't working for you - Quinton
            products = context.Products.ToList();
            technicians = context.Technicians.ToList();
            customers = context.Customers.ToList();
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }


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



            //Send the lists to the view using the ViewBag - Quinton
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

            //Instead of only including the incident -Q
            // var incident = context.Incidents.Find(id);

            //Include all foreign key information, and then filter by the id given - Quinton
            var incident = context.Incidents
                .Include(c => c.Customer)
                .Include(c => c.Technician)
                .Include(c => c.Product)
                .FirstOrDefault(c => c.IncidentID == id);



            //Send the lists to the view using the ViewBag
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

                //Incident shouldn't be in the Redirect - Quinton
                //return RedirectToAction("List", "Incident");

                return RedirectToAction("List");
            }
            else
            {
                /*Not needed because if the ModelState.IsValid check fails, the same 'Edit' view is returned to the user
                 *Because the view was initially rendered for edititing there is no need to change the Action value as it will remain the same.
                 *-Quinton
                 */
                //ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";

                //Also include all necessary lists here
                ViewBag.Customers = customers;
                ViewBag.Technicians = technicians;
                ViewBag.Products = products;

                //No need to send the incident again, same reason as above. -Quinton
                //return View("Edit", incident);

                //Only return the view
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
