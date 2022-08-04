using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Infrastructure;
using ThePeejayAPI.Models;
using ThePeejayAPI.Repositories;
using ThePeejayAPI.Services;

namespace ThePeejayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Cart cart;

        public CartController(IProductRepository productRepository, Cart cartService)
        {
            _productRepository = productRepository;
            cart = cartService;
        }

        [HttpGet]
        public IActionResult Cart()
        {
            return Ok(new CartModel
            {
                Cart = cart
            });
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart(Product product)
        {
            try
            {
                var productFound = await this._productRepository.GetProduct(product.Id);

                if (product != null)
                {
                    Cart cart = GetCart();
                    cart.AddItem(product, 1);
                    SaveCart(cart);
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        [HttpPost]
        [Route("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            try
            {
                var product = await this._productRepository.GetProduct(id);

                if (product != null)
                {
                    Cart cart = GetCart();
                    cart.RemoveLine(product);
                    SaveCart(cart);
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}
