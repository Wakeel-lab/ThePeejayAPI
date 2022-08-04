using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Discount>> GetDiscounts();
        Task<Discount> GetDiscount(int id);
        Task<Product> GetProduct(int id);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
        void SearchProduct(Product product);
    }
}
