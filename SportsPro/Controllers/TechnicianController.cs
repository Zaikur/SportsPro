/*
 * Quinton Nelson 
 * 1/21/2023
 * Controller for the technician portion of the application
 */

using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using SportsPro.Data.DataLayer;
using SportsPro.Data.DataLayer.Repositories;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private Repository<Technician> data { get; set; }
        public TechnicianController(SportsProContext ctx) => data = new Repository<Technician>(ctx);

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("Technicians")]
        public ViewResult List()
        {
            var technicians = data.List(new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID != -1,
                OrderBy = t => t.TechnicianID
            });
            return View(technicians);
        }

        [HttpGet]
        public ViewResult Add() => View("AddEdit", new Technician());

        [HttpPost]
        public IActionResult Add(Technician technician)
        {
            if (ModelState.IsValid)
            {
                data.Insert(technician);
                data.Save();
                return RedirectToAction("List");
            }
            else
            {
                return View("AddEdit", technician);
            }
        }

        [HttpGet]
        public ViewResult Edit(int id) => View("AddEdit", data.Get(id));

        [HttpPost]
        public IActionResult Edit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                data.Update(technician);
                data.Save();
                return RedirectToAction("List");
            }
            else
            {
                return View("AddEdit", technician);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var technician = data.Get(new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID == id
            });
            return View("AddEdit", technician);
        }

        [HttpPost]
        public RedirectToActionResult Delete(Technician technician)
        {
            data.Delete(technician);
            data.Save();
            return RedirectToAction("List");
        }
    }
}