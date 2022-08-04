using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.DTOs;
using ThePeejayAPI.Extensions;
using ThePeejayAPI.Models;
using ThePeejayAPI.Repositories;
using ThePeejayAPI.Services;

namespace ThePeejayAPI.Controllers
{
    [Authorize(Roles = EmployeeRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IDiscountRepository _discountRepository;

        public AdministratorController(IProductRepository productRepository
            , IProductImageRepository productImageRepository, IDiscountRepository discountRepository)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _discountRepository = discountRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ProductDto>> Administrator()
        {
            try
            {
                var allProducts = await _productRepository.GetAllProducts();
                var productCategories = await _productRepository.GetCategories();
                var productDiscounts = await _productRepository.GetDiscounts();


                if (allProducts == null || productCategories == null || productDiscounts == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = allProducts.ConvertToDto(productCategories, productDiscounts);
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database...");
            }


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Product(int id)
        {
            try
            {
                var product = await _productRepository.GetProduct(id);
                var productCategory = await this._productRepository.GetCategory(product.CategoryId);
                var productDiscount = await this._productRepository.GetDiscount(product.DiscountId);

                if (product == null)
                { 
                    return NotFound();
                }
                else
                {
                    

                    var productDto = product.ConvertToDto(productCategory, productDiscount);

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from database");
            }

        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            Product newProduct = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                DiscountId = product.DiscountId,
                Available = product.Available,
                IsFeatured = product.IsFeatured,
                Sku = product.GeneratesSKU(),
                Discount = product.Discount,
                CategoryId = product.CategoryId,
                CoverImage = product.CoverImage,
                CreatedDate = DateTime.UtcNow,
                Quantity = product.Quantity,
                Price = product.Price
            };

            int id = newProduct.DiscountId;

            Discount discount = new Discount();
            

            var discountPercentage = 0.00m;

            if (product.Price!=0.0m && id != 0)
            {

                    var getDiscountById = discount.Id;
                    getDiscountById = id;
                    var extractDiscount = await _discountRepository.GetDiscount(getDiscountById);
                    discountPercentage = extractDiscount.PercentageDiscount;
                    var discountExtract = (product.Price / 100) * discountPercentage;
                    newProduct.PriceAfterDiscount = product.Price - discountExtract;
                
            }

            await _productRepository.Add(newProduct);

            List<ProductImage> productImages = product.ProductImages.ToList();

            foreach (var image in productImages)
            {
                image.ProductId = newProduct.Id;
            }

            _productImageRepository.AddRange(productImages);

            return CreatedAtAction("Product", new { id = newProduct.Id }, newProduct);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (ModelState.IsValid)
            {

                Product existingProduct = await _productRepository.GetProduct(id);

                if (existingProduct == null)
                {
                    return NotFound("Product cannot be found");
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.ModifiedDate = DateTime.UtcNow;
                existingProduct.Available = product.Available;
                existingProduct.IsFeatured = product.IsFeatured;
                existingProduct.Quantity = product.Quantity;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Description = product.Description;
                existingProduct.Discount = product.Discount;
                existingProduct.CoverImage = product.CoverImage;

                int existingProductDiscountId = existingProduct.DiscountId;

                Discount discount = new Discount();


                var discountPercentage = 0.00m;

                if (product.Price != 0.0m && existingProductDiscountId != 0)
                {
                    var extractDiscount = await _discountRepository.GetDiscount(existingProduct.DiscountId);
                    discountPercentage = extractDiscount.PercentageDiscount;
                    var discountExtract = (product.Price / 100) * discountPercentage;
                    existingProduct.PriceAfterDiscount = product.Price - discountExtract;

                }

                await _productRepository.UpdateProduct(existingProduct);


                List<ProductImage> productImages = product.ProductImages.ToList();
               

                await _productImageRepository.UpdateProductImageRange(productImages);
                return StatusCode(StatusCodes.Status202Accepted, "Update Successful");
            }
            return BadRequest("Some Fields are missing");
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductDelete(int id, Product product)
        {
            var productFound = await _productRepository.GetProduct(id);

            if (productFound != null)
            {
                await _productRepository.DeleteProduct(productFound.Id);
            }

            var productImages = product.ProductImages.ToList();

            await _productImageRepository.DeleteProductImageRange(productImages);
            return StatusCode(StatusCodes.Status200OK, "Product successfully deleted");

        }
    }
}
