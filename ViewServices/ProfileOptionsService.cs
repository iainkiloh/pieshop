using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Pieshop.ViewServices
{
    public class ProfileOptionsService
    {
      
        public List<SelectListItem> ListCountries()
        {
            return new List<SelectListItem>() {
                new SelectListItem{ Text = "Scotland", Value = "Scotland" },
                new SelectListItem{ Text = "England", Value = "England" },
                new SelectListItem{ Text = "Wales", Value = "Wales" },
                new SelectListItem{ Text = "Ireland", Value = "Ireland" },
                new SelectListItem{ Text = "Northen Ireland", Value = "Northen Ireland" }
            };
        }

        public List<SelectListItem> ListRoles()
        {
            return new List<SelectListItem>() {
                new SelectListItem{ Text = "Admin", Value = "Admin" },
                new SelectListItem{ Text = "Staff", Value = "Staff" },
                new SelectListItem{ Text = "Customer", Value = "Customer" }
            };
        }

    }
}
