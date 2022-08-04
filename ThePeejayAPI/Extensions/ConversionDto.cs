using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.DTOs;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Extensions
{
    public static class ConversionDto
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
        IEnumerable<Category> categories, IEnumerable<Discount> discounts)
        {
            return (from product in products
                    join category in categories
                    on product.CategoryId equals category.Id
                    join discount in discounts on product.DiscountId equals discount.Id
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CoverImage = product.CoverImage,
                        Price = product.Price,
                        Sku = product.Sku,
                        PriceAfterDiscounted = product.PriceAfterDiscount,
                        DiscountName = discount.Name,
                        Quantity = product.Quantity,
                        CategoryName = category.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product,
        Category category, Discount discount)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CoverImage = product.CoverImage,
                Price = product.Price,
                Sku = product.Sku,
                PriceAfterDiscounted = product.PriceAfterDiscount,
                DiscountName = discount.Name,
                Quantity = product.Quantity,
                CategoryName = category.Name
            };
        }
    }
}
