using Microsoft.AspNetCore.Http;
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
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Discounts()
        {
            IEnumerable<Discount> allDiscounts = await discountRepository.GetAllDiscounts();
            return Ok(allDiscounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Discount(int id)
        {
            var discount = await discountRepository.GetDiscount(id);
            return StatusCode(StatusCodes.Status200OK, discount);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] Discount discount)
        {
            var newDiscount = new Discount()
            {
                Name = discount.Name,
                Description = discount.Description,
                PercentageDiscount = discount.PercentageDiscount,
                CreatedDate = DateTime.UtcNow
            };

            var discountCreated = await discountRepository.Add(newDiscount);

            return CreatedAtAction("Discount", new { id = discountCreated.Id }, discountCreated);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DiscountUpdate(int id, [FromBody] Discount discount)
        {
            var existingDiscount = await discountRepository.GetDiscount(id);

            if (existingDiscount == null)
            {
                return NotFound("Discount cannot be found");
            }

            existingDiscount.Name = discount.Name;
            existingDiscount.ModifiedDate = DateTime.UtcNow;


            await discountRepository.UpdateDiscount(existingDiscount);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discountToBeDeleted = await discountRepository.GetDiscount(id);

            if (discountToBeDeleted != null)
            {
                await discountRepository.DeleteDiscount(discountToBeDeleted.Id);

                return StatusCode(StatusCodes.Status200OK);
            }
            else
                return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}

