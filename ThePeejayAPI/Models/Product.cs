using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Models
{
    public class Product
    {
        public Product()
        {
            ProductImages = new HashSet<ProductImage>();
            //ProductRatings = new HashSet<ProductRating>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The product name must not exceed 50 characters.")]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string CoverImage { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "900000", ErrorMessage = "Invalid Price entered")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(16,2)")]
        public decimal PriceAfterDiscount { get; set; }
        public string Sku { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        public int DiscountId { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The description must not exceed 50 characters.")]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Category CategoryNavigation { get; set; }
        public Discount Discount { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        //public virtual ICollection<ProductRating> ProductRatings { get; set; }
    }
}
