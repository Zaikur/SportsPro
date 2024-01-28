/*
 * Quinton Nelson 
 * 1/21/2023
 * Controller for the customer portion of the application
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private SportsProContext context;
        private List<Country> countries;

        public CustomerController(SportsProContext ctx)
        {
            context = ctx;
            countries = context.Countries
                .OrderBy (country => country.Name)
                .ToList();
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var customers = context.Customers
                .OrderBy(c => c.CustomerID).ToList();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            Customer customer = new Customer();

            ViewBag.Action = "Add";
            ViewBag.Countries = countries;

            return View("AddEdit", customer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Customer? customer = context.Customers
                .Include(c => c.Country)
                .FirstOrDefault(c => c.CustomerID == id);

            ViewBag.Action = "Edit";
            ViewBag.Countries = countries;

            return View("AddEdit", customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    context.Customers.Add(customer);
                }
                else
                {
                    context.Customers.Update(customer);
                }
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = "Save";
                ViewBag.Countries = countries;
                return View("AddEdit");
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customer? customer = context.Customers.Find(id);
            return View(customer);
        }


        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
