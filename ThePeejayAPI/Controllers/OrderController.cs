using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;
using ThePeejayAPI.Repositories;

namespace ThePeejayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOrderRepository _orderRepository;

        public OrderController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(string name, Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userName = await _userManager.FindByNameAsync(name);

                    if (userName != null)
                    {
                        var order1 = new Order
                        {
                            Address1 = order.Address1,
                            Address2 = order.Address2,
                            Address3 = order.Address3,
                            City = order.City,
                            StateOrProvince = order.StateOrProvince,
                            Zip = order.Zip,
                            GiftWrap = order.GiftWrap,
                            OrderedDate = DateTime.Now,
                            Items = order.Items.ToList()
                        };
                        _orderRepository.SaveOrder(order1);
                    }
                    else
                    {
                        return RedirectToAction("LogIn", "Account");
                    }
                }

                return StatusCode(StatusCodes.Status202Accepted);
                
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }
    }
}
