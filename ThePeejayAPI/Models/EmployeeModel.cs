using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Services;

namespace ThePeejayAPI.Models
{
    public class EmployeeModel
    {
        [Display(Name = "First Name")]
        [StringLength(10, ErrorMessage = "Middle name must not exceed 10 characters.")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        [StringLength(10, ErrorMessage = "Middle name must not exceed 10 characters.")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(10, ErrorMessage = "Last name must not exceed 10 characters.")]
        public string LastName { get; set; }
        [Required]
        [ValidEmailDomain(allowedDomain: ("peejay.com"),
            ErrorMessage = "Email domain must be @peejay.com")]
        public string Email { get; set; }
        public Department Department { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Photo { get; set; }
    }
}
