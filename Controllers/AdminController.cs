using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Pieshop.Models;
using Pieshop.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult UserManagement()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new ApplicationUser
            {
                UserName = vm.UserName,
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                City = vm.City,
                Country = vm.Country
            };

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                if (vm.Roles.Any())
                {
                    foreach (var role in vm.Roles.Where(x => x.Selected))
                    {
                        await _userManager.AddToRoleAsync(user, role.Text);
                    }
                }

                return RedirectToAction("UserManagement", _userManager.Users);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
           
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }

            //map to EditUservm 
            var vm = new EditUserViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = new List<SelectListItem>()
            };

            //fetch roles and add to view model
            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                vm.Roles.Add(new SelectListItem { Selected = true, Text = role, Value = role });
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }

            //set properties which are specific to the vm to the user
            user.UserName = vm.UserName;
            user.Email = vm.Email;
            user.BirthDate = vm.BirthDate;
            user.City = vm.City;
            user.Country = vm.Country;
            user.Email = vm.Email;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.UserName = vm.UserName;
           
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                IdentityResult roleRemoveResult = null;
                IdentityResult roleAddResult = null;

                //check roles
                var savedUserRoles = await _userManager.GetRolesAsync(user);

                //is user in any savedRoles which are not in the vm selected = true list of roles?
                var rolesToRemove = savedUserRoles.Where(p => !vm.Roles.Where(x => x.Selected).Select(x => x.Value).Contains(p)).ToList();
                if(rolesToRemove.Any())
                {
                    roleRemoveResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    //TODO - log any errors
                }
                //if user has new roles which he is not currently in then add them
                var rolesToAdd = vm.Roles.Where(x => x.Selected).Select(x => x.Value).Where(p => !savedUserRoles.Contains(p)).ToList();
                if (rolesToAdd.Any())
                {
                    roleAddResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    //TODO - log any errors
                }

                return RedirectToAction("UserManagement", _userManager.Users);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
           

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "Unable to find user");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserManagement", _userManager.Users);
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong while deleting the user");
                }
            }

            return View("UserManagement", _userManager.Users);

        }



    }
}
