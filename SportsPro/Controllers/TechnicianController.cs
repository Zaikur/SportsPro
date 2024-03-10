/*
 * Quinton Nelson 
 * 1/21/2023
 * Controller for the technician portion of the application
 */

using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private SportsProContext context;

        public TechnicianController(SportsProContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("Technicians")]
        public IActionResult List()
        {
            var technicians = context.Technicians
                .Where(t => t.TechnicianID != -1)
                .OrderBy(t => t.TechnicianID).ToList();
            return View(technicians);
        }

        [HttpGet]
        public IActionResult Add()
        {
            Technician technician = new Technician();

            ViewBag.Action = "Add";

            return View("AddEdit", technician);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Technician? technician = context.Technicians
                .FirstOrDefault(c => c.TechnicianID == id);

            ViewBag.Action = "Edit";

            return View("AddEdit", technician);
        }

        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                if (technician.TechnicianID == 0)
                {
                    context.Technicians.Add(technician);
                }
                else
                {
                    context.Technicians.Update(technician);
                }
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Save";
                return View("AddEdit");
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Technician? technician = context.Technicians.Find(id);
            return View(technician);
        }


        [HttpPost]
        public IActionResult Delete(Technician technician)
        {
            context.Technicians.Remove(technician);
            context.SaveChanges();
            return RedirectToAction("List");
        }


    }
}
