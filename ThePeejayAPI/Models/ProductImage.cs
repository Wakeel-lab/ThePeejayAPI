using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? ProductId { get; set; }
        [Required]
        public string URL { get; set; }

        public virtual Product Product { get; set; }
    }
}
