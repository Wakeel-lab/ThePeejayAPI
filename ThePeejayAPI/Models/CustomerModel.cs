using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Models
{
    public class CustomerModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Username must not exceed 10 characters.")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
