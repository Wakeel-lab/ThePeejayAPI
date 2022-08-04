using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Services;

namespace ThePeejayAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name ="Middle Name")]
        [StringLength(10, ErrorMessage ="Middle name must not exceed 10 characters.")]
        public string MiddleName { get; set; }
        [Display(Name ="Last Name")]
        [StringLength(10, ErrorMessage = "Last name must not exceed 10 characters.")]
        public string LastName { get; set; }
        public Department Department { get; set; }
        public string City { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Photo { get; set; }
    }
}
