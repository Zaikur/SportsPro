/* Ayden Hofts
 * 04/14/2024
 * Modifyed the ProductController to use the IRepository to grab the data.
 */



using Microsoft.AspNetCore.Mvc;
using SportsPro.Data.DataLayer.Repositories;
using SportsPro.Models;
using SportsPro.Data.DataLayer;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public RedirectToActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Route("products")]
        public ViewResult List()
        {
            var products = _productRepository.List(new QueryOptions<Product> { OrderBy = p => p.ReleaseDate });
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
            var product = _productRepository.Get(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    _productRepository.Insert(product);
                    _productRepository.Save();
                    TempData["UserMessage"] = product.Name + " was added.";
                }
                else
                {
                    _productRepository.Update(product);
                    _productRepository.Save();
                    TempData["UserMessage"] = product.Name + " was edited.";
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
            var product = _productRepository.Get(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _productRepository.Delete(product);
            _productRepository.Save();
            TempData["UserMessage"] = "Product deleted";
            return RedirectToAction("List", "Product");
        }
    }
}
