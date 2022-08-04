using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayView.ViewModels
{
    public class CreateProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name length must not exceed 100")]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Range(typeof(decimal), "1", "900000", ErrorMessage = "Invalid Price entered")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Sku { get; set; }
        [Required]
        public bool IsFeatured { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "900000", ErrorMessage = "Invalid Price entered")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public int DiscountId { get; set; }
        public string CoverImageURL { get; set; }
        public IFormFile CoverImage { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public List<IFormFile> ProductImages { get; set; }

        public List<ProductImagesViewModel> Images { get; set; }
    }
}
