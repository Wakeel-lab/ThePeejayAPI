using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext context;

        public DiscountRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Discount> Add(Discount discount)
        {
            var result = await context.Discounts.AddAsync(discount);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Discount> DeleteDiscount(int id)
        {
            var discountFound = await context.Discounts.FindAsync(id);

            if (discountFound != null)
            {
                context.Discounts.Remove(discountFound);
                await context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<Discount> EditDiscount(int id)
        {
            var discountToEdit = await context.Discounts.FindAsync(id);
            var newDiscount = new Discount();

            if (discountToEdit != null)
            {
                discountToEdit.Name = newDiscount.Name;
                discountToEdit.ModifiedDate = DateTime.UtcNow;

            }

            await context.Discounts.AddAsync(discountToEdit);
            await context.SaveChangesAsync();

            return discountToEdit;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscounts() => await context.Discounts.ToListAsync();

        public async Task<Discount> GetDiscount(int id)
        {
            return await context.Discounts.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Discount> UpdateDiscount(Discount discount)
        {
            var disc = context.Discounts.Attach(discount);
            disc.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return discount;
        }
    }
}
