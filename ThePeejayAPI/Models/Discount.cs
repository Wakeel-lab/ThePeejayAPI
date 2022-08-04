using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Models
{
    public class Discount
    {
        public Discount()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="The discount name must not exceed 50 characters.")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The description must not exceed 50 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage ="The percentage discount is required and should be an integer like 1, 2, 3 and so on.")]
        public int PercentageDiscount { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

}
