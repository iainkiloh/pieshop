using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
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
                FirstName = vm.Firstname,
                LastName = vm.Lastname,
                BirthDate = vm.Birthdate,
                City = vm.City, 
                Country = vm.Country
            };

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                if (vm.Roles.Any())
                {
                    foreach (var role in vm.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
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
                Birthdate = user.BirthDate,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                UserName = user.UserName,
                Roles = new List<string>()
            };

            vm.Roles = (await _userManager.GetRolesAsync(user)).ToList();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null)
            {
                return RedirectToAction("UserManagement", _userManager.Users);
            }

            user.UserName = vm.UserName;
            user.Email = vm.Email;
            user.BirthDate = vm.Birthdate;
            user.City = vm.City;
            user.Country = vm.Country;
            user.Email = vm.Email;
            user.FirstName = vm.Firstname;
            user.LastName = vm.Lastname;
            user.UserName = vm.UserName;
           
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                IdentityResult roleRemoveResult = null;
                IdentityResult roleAddResult = null;

                //check roles
                var savedUserRoles = await _userManager.GetRolesAsync(user);

                //is user in any savedRoles which are not in the vm list of roles?
                var rolesToRemove = savedUserRoles.Where(p => !vm.Roles.Contains(p)).ToList();
                if(rolesToRemove.Any())
                {
                    roleRemoveResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    //TODO - log any errors
                }
                //if user has new roles which he is not currently in then add them
                var rolesToAdd = vm.Roles.Where(p => !savedUserRoles.Contains(p)).ToList();
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
