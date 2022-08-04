using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;
using ThePeejayView.Services;

namespace ThePeejayView.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult CreateProduct()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> CreateProduct()
        {
            var prod = new Product()
            {
                Name = "Rizza",
                Description = "elaborate",
                DiscountId = 5,
                CategoryId = 5,
                CreatedDate = DateTime.Now,
                Sku = "12344",
                Available = true,
                IsFeatured = true,
                CoverImage = "rizza.jpg",
                Price = 19.00m,
                Quantity = 90
            };
            var productCreated = await _productService.CreateProduct(prod);

            if (productCreated == null)
            {
                return Problem("Error uploading product to the server");
            }
            else
                return RedirectToAction("Index");
        }
    }
}
