using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Services
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
            .Where(p => p.Product.Id == product.Id)
            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Product product) =>
        lineCollection.RemoveAll(l => l.Product.Id == product.Id);

        public virtual decimal CalculateTotal(Product product, Discount discount, int quantity)
        {
            var discountInDecimalNumber = discount.PercentageDiscount / 100;
            var discountedAmount = product.Price * (decimal)discountInDecimalNumber;

            var newPriceAfterDiscount = product.Price - discountedAmount;

            return newPriceAfterDiscount;
        }

        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }
}
