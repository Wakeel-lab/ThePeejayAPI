using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Add(Category category);
        Task<Category> DeleteCategory(int id);
        Task<Category> UpdateCategory(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<Category> EditCategory(int id);
    }
}
