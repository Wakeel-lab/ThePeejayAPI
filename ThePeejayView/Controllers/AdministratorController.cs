using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;
using ThePeejayView.Services;
using ThePeejayView.ViewModels;

namespace ThePeejayView.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly ILogger<AdministratorController> _logger;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdministratorController(ILogger<AdministratorController> logger, IProductService productService
                              , IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    DiscountId = model.DiscountId,
                    Available = model.Available,
                    IsFeatured = model.IsFeatured,
                    CategoryId = model.CategoryId,
                    Quantity = model.Quantity,
                    Price = model.Price
                };
                if (model.CoverImage != null)
                {
                    string folder = "coverimages";
                    await UploadImage(folder, model.CoverImage);
                }

                if (model.ProductImages != null)
                {
                    string folder = "productImages";
                    model.Images = new List<ProductImagesViewModel>();
                    foreach (IFormFile prodImage in model.ProductImages)
                    {
                        var image = new ProductImagesViewModel()
                        {
                            Name = prodImage.FileName,
                            URL = await UploadImage(folder, prodImage)
                        };

                        model.Images.Add(image);
                    }
                }

                await _productService.CreateProduct(product);
                return RedirectToAction(nameof(CreateProduct), new { isSuccess = true });
            }
            return View();


            //Product newProduct = new Product
            //{
            //    Name = "feli",
            //    Description = "lovely",
            //    DiscountId = 2,
            //    CategoryId = 2,
            //    Price = 2.00m,
            //    CoverImage = "felli.jpg",
            //    IsFeatured = true,
            //    Available = true,
            //    Quantity = 5
            //};

            //List<ProductImage> productImage = new()
            //{
            //    new ProductImage()
            //    {
            //        Name = "leet.jpg",
            //        URL = "productImages/leet.jpg"
            //    },
            //    new ProductImage()
            //    {
            //        Name = "kl.jpg",
            //        URL = "productImages/kl.jpg"
            //    }

            //};

            //if (newProduct.ProductImages != null)
            //{


            //    foreach (var item in productImage)
            //    {
            //        newProduct.ProductImages.Add(item);
            //    }

            //}
            //await _productService.CreateProduct(newProduct);
            //return View();

        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(uploadsFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}
