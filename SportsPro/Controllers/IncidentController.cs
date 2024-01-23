/*
 * Ayden Hofts
 * 01/22/2024
 * This is the controller for the incident section of the website
 */



using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SportsPro.Controllers

{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx) => context = ctx;

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
            return View("Edit", new Incident());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Display the Add/Edit Incident page with the data for the selected incident
            ViewBag.Action = "Edit";
            var incident = context.Incidents.Find(id);
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
                return RedirectToAction("List", "Incident");
            }
            else
            {
                ViewBag.Action = (incident.IncidentID == 0) ? "Add" : "Edit";
                return View("Edit", incident);
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
