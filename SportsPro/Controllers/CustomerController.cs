/*
 * Quinton Nelson 
 * 1/21/2023
 * Controller for the customer portion of the application
 * 
 * Quinton Nelson
 * 04/14/2024
 * Modified the Customer Controller to use the SportsProUnitOfWork class to access the database
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Data.DataLayer.Repositories;
using SportsPro.Data.DataLayer;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private SportsProUnitOfWork data { get; set; }

        public CustomerController(SportsProContext ctx)
        {
            data = new SportsProUnitOfWork(ctx);
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("customers")]
        public IActionResult List()
        {
            var options = new QueryOptions<Customer>
            {
                OrderBy = c => c.CustomerID
            };

            var customers = data.Customers.List(options);
            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            Customer customer = new Customer();

            ViewBag.Action = "Add";
            ViewBag.Countries = data.Countries.List(new QueryOptions<Country>());

            return View("AddEdit", customer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var options = new QueryOptions<Customer>
            {
                Includes = "Country",
                Where = c => c.CustomerID == id
            };

            Customer? customer = data.Customers.Get(options);

            ViewBag.Action = "Edit";
            ViewBag.Countries = data.Countries.List(new QueryOptions<Country>());
            return View("AddEdit", customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    data.Customers.Insert(customer);
                }
                else
                {
                    data.Customers.Update(customer);
                }
                data.Save();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Action = customer.CustomerID == 0 ? "Add" : "Edit";
                ViewBag.Countries = data.Countries.List(new QueryOptions<Country>());
                return View("AddEdit", customer);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customer? customer = data.Customers.Get(id);
            return View(customer);
        }


        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            data.Customers.Delete(customer);
            data.Save();
            return RedirectToAction("List");
        }
    }
}
