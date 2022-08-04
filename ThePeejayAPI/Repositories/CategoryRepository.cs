using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Category> Add(Category category)
        {
            var result = await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return result.Entity;

        }

        public async Task<Category> DeleteCategory(int id)
        {
            var categoryFound = await context.Categories.FindAsync(id);

            if (categoryFound != null)
            {
                context.Categories.Remove(categoryFound);
                await context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<Category> EditCategory(int id)
        {
            var categoryToEdit = await context.Categories.FindAsync(id);
            Category newCategory = new Category();

            if (categoryToEdit != null)
            {
                categoryToEdit.Name = newCategory.Name;
                categoryToEdit.ModifiedDate = DateTime.UtcNow;

            }

            await context.Categories.AddAsync(categoryToEdit);
            await context.SaveChangesAsync();

            return categoryToEdit;
        }

        public async Task<IEnumerable<Category>> GetAllCategories() => await context.Categories.ToListAsync();

        public async Task<Category> GetCategory(int id) => await context.Categories.FirstOrDefaultAsync(c => c.Id == id);


        public async Task<Category> UpdateCategory(Category category)
        {
            var cat = context.Categories.Attach(category);
            cat.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return category;
        }
    }
}
