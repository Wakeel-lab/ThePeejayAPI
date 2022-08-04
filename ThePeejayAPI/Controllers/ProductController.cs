using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;

        public ProductController(IProductRepository providerTyresRepository, IProductImageRepository productImageRepository)
        {
            _productRepository = providerTyresRepository;
            _productImageRepository = productImageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Products()
        {
            try
            {
                IEnumerable<Product> allProducts = await _productRepository.GetAllProducts();
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

                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    var productCategory = await this._productRepository.GetCategory(product.CategoryId);
                    var productDiscount = await this._productRepository.GetDiscount(product.DiscountId);

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
    }
}
