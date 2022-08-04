using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CoverImage { get; set; }
        public int Quantity { get; set; }
        public string Sku { get; set; }
        public string DiscountName { get; set; }
        public decimal PriceAfterDiscounted { get; set; }
        public string CategoryName { get; set; }
    }
}
