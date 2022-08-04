using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Repositories
{
    public interface IProductImageRepository
    {
        void AddRange(List<ProductImage> ProductImages);
        Task<ProductImage> UpdateProductImageRange(List<ProductImage> productImages);
        Task<ProductImage> DeleteProductImageRange(List<ProductImage> productImages);
    }
}
