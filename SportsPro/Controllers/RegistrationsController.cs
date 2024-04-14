/* 
 * Jason Nelson
 * 04/11/2024
 * Registration controller to handle product registrations/deletions
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Data.DataLayer;
using SportsPro.Data.DataLayer.Repositories;
using SportsPro.Models;
using SportsPro.ViewModels;

namespace SportsPro.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly SportsProUnitOfWork _unitOfWork;

        public RegistrationsController(SportsProContext ctx)
        {
            _unitOfWork = new SportsProUnitOfWork(ctx);
        }

        [Route("registration/getcustomer")]
        [HttpGet]
        public IActionResult GetCustomer()
        {
            var model = new CustomerViewModel
            {
                Customers = _unitOfWork.Customers.List(new QueryOptions<Customer>()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Route("registration/getcustomer")]
        public IActionResult GetCustomer(CustomerViewModel model)
        {
            var customer = _unitOfWork.Customers.Get(model.SelectedCustomerId);

            if (customer != null)
            {
                HttpContext.Session.SetInt32("SelectedCustomerId", model.SelectedCustomerId);
                HttpContext.Session.SetString("CustomerName", customer.FullName);
            }

            return RedirectToAction("ListByCustomer", new { id = model.SelectedCustomerId });
        }

        [Route("registrations/{id?}")]
        [HttpGet]
        public IActionResult ListByCustomer(int? id)
        {
            id ??= HttpContext.Session.GetInt32("SelectedCustomerId") ?? 0;

            if (id == 0)
            {
                return RedirectToAction("GetCustomer");
            }

            var customer = _unitOfWork.Customers.Get(id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.CustomerName = customer.FullName;

            var viewModel = new CustomerViewModel
            {
                SelectedCustomerId = customer.CustomerID,
                SelectedCustomerName = customer.FullName,
                Registrations = customer.Registrations.ToList(),
                Products = _unitOfWork.Products.List(new QueryOptions<Product>()).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RegisterProduct(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("SelectedCustomerId");

            if (customerId == null)
            {
                return RedirectToAction("GetCustomer");
            }

            var customer = _unitOfWork.Customers.Get(customerId.Value);

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

            _unitOfWork.Registrations.Insert(registration);
            _unitOfWork.Save();

            return RedirectToAction("ListByCustomer", new { id = customerId });
        }

        [HttpPost]
        public IActionResult DeleteRegistration(int productId, int customerId)
        {
            var options = new QueryOptions<Registration>
            {
                Where = r => r.CustomerID == customerId && r.ProductID == productId
            };

            var registration = _unitOfWork.Registrations.List(options).FirstOrDefault();

            if (registration != null)
            {
                _unitOfWork.Registrations.Delete(registration);
                _unitOfWork.Save();
                TempData["Message"] = "Product registration deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Registration not found.";
            }

            return RedirectToAction("ListByCustomer", new { id = customerId });
        }
    }
}
