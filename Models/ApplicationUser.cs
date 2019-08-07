using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pieshop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [PersonalData, Required]
        public DateTime BirthDate { get; set; }

        [PersonalData, Required, StringLength(20)]
        public string FirstName { get; set; }

        [PersonalData, Required, StringLength(20)]
        public string LastName { get; set; }
    }
}
