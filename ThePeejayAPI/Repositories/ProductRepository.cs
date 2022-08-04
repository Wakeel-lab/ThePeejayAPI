using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext context;
        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Product> Add(Product product)
        {
            var result = await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            Product findProduct = await context.Products.FindAsync(id);
            if (findProduct != null)
            {
                context.Products.Remove(findProduct);
                await context.SaveChangesAsync();
            }

            return findProduct;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await context.Products.Include(p => p.ProductImages).ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var prod = context.Products.Attach(product);
            prod.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> GetProduct(int id)
        {
            return await context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
        }

        public void SearchProduct(Product product)
        {
            context.Products.AsQueryable().All(p => p.Name == product.Name);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Discount>> GetDiscounts() => await context.Discounts.ToListAsync();
        public async Task<Discount> GetDiscount(int id) => await context.Discounts.SingleOrDefaultAsync(c => c.Id == id);

    }
}
