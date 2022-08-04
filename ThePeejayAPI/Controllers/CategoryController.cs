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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            IEnumerable<Category> allCategories = await categoryRepository.GetAllCategories();
            return Ok(allCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Category(int id)
        {
            var category = await categoryRepository.GetCategory(id);
            return StatusCode(StatusCodes.Status200OK, category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            var newCategory = new Category()
            {
                Name = category.Name,
                CreatedDate = DateTime.UtcNow
            };

            var categoryAdded = await categoryRepository.Add(newCategory);

            return CreatedAtAction("Category", new { id = categoryAdded.Id }, categoryAdded);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> CategoryUpdate(int id, [FromBody] Category category)
        {
            var existingCategory = await categoryRepository.GetCategory(id);

            if (existingCategory == null)
            {
                return NotFound("Category cannot be found");
            }

            existingCategory.Name = category.Name;
            existingCategory.ModifiedDate = DateTime.UtcNow;


            await categoryRepository.UpdateCategory(existingCategory);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryToBeDeleted = await categoryRepository.GetCategory(id);

            if (categoryToBeDeleted != null)
            {
                await categoryRepository.DeleteCategory(categoryToBeDeleted.Id);

                return StatusCode(StatusCodes.Status200OK);
            }
            else
                return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
