using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public interface IDiscountRepository
    {
        Task<Discount> Add(Discount discount);
        Task<Discount> DeleteDiscount(int id);
        Task<Discount> UpdateDiscount(Discount discount);
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task<Discount> GetDiscount(int id);
        Task<Discount> EditDiscount(int id);
    }
}
