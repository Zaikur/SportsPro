/* 
 * Jason Nelson
 * 04/11/2024
 * Registration controller to handle product registrations/deletions
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.ViewModels;

namespace SportsPro.Controllers
{
    public class RegistrationsController : Controller
    {
        private SportsProContext context { get; set; }

        private List<Product> products;
        private List<Customer> customers;

        public RegistrationsController(SportsProContext ctx)
        {
            context = ctx;

            products = context.Products.ToList();
            customers = context.Customers.ToList();
        }

        // Redirect to the GetCustomer action when accessing the Index page
        public RedirectToActionResult Index()
        {
            return RedirectToAction("GetCustomer");
        }

        // Display the GetCustomer page to select a customer for product registration
        [Route("registration/getcustomer")]
        [HttpGet]
        public IActionResult GetCustomer()
        {
            var model = new CustomerViewModel()
            {
                Customers = context.Customers.ToList()
            };

            return View(model);
        }

        // Process the selected customer and redirect to the ListByCustomer action
        [HttpPost]
        [Route("registration/getcustomer")]
        public IActionResult GetCustomer(CustomerViewModel model)
        {
            var customer = context.Customers.Find(model.SelectedCustomerId);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("SelectedCustomerId", model.SelectedCustomerId);
                HttpContext.Session.SetString("CustomerName", context.Customers.Find(model.SelectedCustomerId).FullName);
            }

            return RedirectToAction("ListByCustomer", new { id = model.SelectedCustomerId });
        }

        // Display a list of registrations for the selected customer
        [Route("registrations/{id?}")]
        [HttpGet]
        public IActionResult ListByCustomer(int? id)
        {
            id ??= HttpContext.Session.GetInt32("SelectedCustomerId") ?? 0;

            if (id == 0)
            {
                return RedirectToAction("GetCustomer");
            }

            var customer = context.Customers
                                    .Include(c => c.Registrations)
                                    .FirstOrDefault(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.CustomerName = customer.FullName;

            var registrations = customer.Registrations.ToList();

            var registeredProductIds = registrations.Select(r => r.ProductID).ToList();
            var products = context.Products
                                    .Where(p => !registeredProductIds.Contains(p.ProductID))
                                    .ToList();

            var viewModel = new CustomerViewModel
            {
                SelectedCustomerId = customer.CustomerID,
                SelectedCustomerName = customer.FullName,
                Registrations = registrations,
                Products = products
            };

            return View(viewModel);
        }

        // Register a product for the selected customer
        [HttpPost]
        public IActionResult RegisterProduct(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("SelectedCustomerId");

            if (customerId == null)
            {
                return RedirectToAction("GetCustomer");
            }

            var customer = context.Customers
                            .Include(c => c.Registrations)
                            .FirstOrDefault(c => c.CustomerID == customerId);

            if (customer == null)
            {
                return RedirectToAction("GetCustomer");
            }

            if (customer.Registrations.Any(r => r.ProductID == productId))
            {
                TempData["Message"] = "Product is already registered for this customer.";
                return RedirectToAction("ListByCustomer", new { id = customerId });
            }

            var registration = new Registration
            {
                CustomerID = customerId.Value,
                ProductID = productId
            };

            context.Registrations.Add(registration);
            context.SaveChanges();

            return RedirectToAction("ListByCustomer", new { id = customerId });
        }

        // Delete a product registration for the selected customer
        [HttpPost]
        public IActionResult DeleteRegistration(int productId, int customerId)
        {
            var registration = context.Registrations
                .FirstOrDefault(r => r.CustomerID == customerId && r.ProductID == productId);

            if (registration == null)
            {
                TempData["Message"] = "Registration not found.";
            }
            else
            {
                context.Registrations.Remove(registration);
                context.SaveChanges();
                TempData["Message"] = "Product registration deleted successfully.";
            }

            return RedirectToAction("ListByCustomer", new { id = customerId });
        }


    }
}
