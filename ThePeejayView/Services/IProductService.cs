using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayView.Services
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
    }
}
