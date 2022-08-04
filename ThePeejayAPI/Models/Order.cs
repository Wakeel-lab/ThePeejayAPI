using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        [BindNever]
        public ICollection<CartLine> Items { get; set; }
        [Required(ErrorMessage = "Please enter the first address")]
        [StringLength(70, ErrorMessage ="Your address 1 only allows for a 70 character length.")]
        public string Address1 { get; set; }
        [StringLength(50, ErrorMessage ="Your address 2 only allows for a 50 character length.")]
        public string Address2 { get; set; }
        [StringLength(50, ErrorMessage = "Your address 2 only allows for a 50 character length.")]
        public string Address3 { get; set; }
        [Required(ErrorMessage ="Please enter a City")]
        public string City { get; set; }
        [Required(ErrorMessage ="Please enter a State or a Province")]
        [Display(Name= "Province or State")]
        public string StateOrProvince { get; set; }
        public string Zip { get; set; }
        [Required]
        public DateTime OrderedDate { get; set; }
        public bool GiftWrap { get; set; }
    }
}
