using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddRange(List<ProductImage> productImages)
        {
            _context.ProductImages.Include(p => p.Product);
            _context.ProductImages.AddRange(productImages);
            _context.SaveChanges();
        }

        public async Task<ProductImage> DeleteProductImageRange(List<ProductImage> productImages)
        {
            foreach (var item in productImages)
            {
                var result = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == item.Id);
                if (result != null)
                {
                    _context.ProductImages.Remove(result);
                    await _context.SaveChangesAsync();
                }
            }
            return null;
        }

        public async Task<ProductImage> UpdateProductImageRange(List<ProductImage> productImages)
        {
            foreach (var item in productImages)
            {
                var result = await _context.ProductImages.FirstOrDefaultAsync(p => p.Id == item.Id);
                if (result != null)
                {
                    result.Name = item.Name;
                    await _context.SaveChangesAsync();
                }
            }
            return null;
        }
    }
}
