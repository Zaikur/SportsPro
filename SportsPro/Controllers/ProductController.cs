/*
 * Jason Nelson
 * 01/19/2024
 * Controller for the product portion of the SportsPro application
 */


using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private SportsProContext context { get; set; }

        public ProductController(SportsProContext ctx) => context = ctx;

        public RedirectToActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("products")]
        public IActionResult List()
>>>>>>> 33c2f7259e836a6238828589863c929fc03ecefa
        {
            var products = context.Products.OrderBy(p => p.ReleaseDate).ToList();
            return View(products);
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Product());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    context.Products.Add(product);
                    context.SaveChanges();
                    TempData["UserMessage"] = product.Name + " added.";
                }
                else
                {
                    context.Update(product);
                    context.SaveChanges();
                    TempData["UserMessage"] = product.Name + " edited.";
                }
                return RedirectToAction("List", "Product");
            }
            else
            {
                ViewBag.Action = (product.ProductID == 0) ? "Add" : "Edit";
                return View(product);
            }
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            var product = context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
            TempData["UserMessage"] = "Product deleted";
            return RedirectToAction("List", "Product");
        }
    }
}
