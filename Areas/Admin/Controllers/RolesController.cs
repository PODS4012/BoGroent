using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BoGroent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoGroent.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        //GET /admin/roles
        public IActionResult Index() => View(roleManager.Roles);

        //GET /admin/roles/create
        public IActionResult Create() => View();

        //POST /admin/roles/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([MinLength(2), Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "The role has been created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors) ModelState.AddModelError("", error.Description);
                }
            }

            ModelState.AddModelError("", "Minimum length is 2");
            return View();
        }

        //GET /admin/roles/edituserrole
        public async Task<IActionResult> EditUserRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();

            foreach (AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            RoleUserEdit roleUserEdit = new RoleUserEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(roleUserEdit);
        }

        //POST /admin/roles/edituserrole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRole(RoleUserEdit roleEdit)
        {
            IdentityResult result;

            foreach (string userId in roleEdit.AddIds ?? new string[] { })
            {
                AppUser user = await userManager.FindByIdAsync(userId);
                result = await userManager.AddToRoleAsync(user, roleEdit.RoleName);
            }

            foreach (string userId in roleEdit.DeleteIds ?? new string[] { })
            {
                AppUser user = await userManager.FindByIdAsync(userId);
                if ( user.UserName == "superadmin")
                {
                    TempData["Error"] = "The admin role from superadmin user cannot be dropped!";
                }
                else
                {
                    AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
                    if (user.UserName == appUser.UserName)
                    {
                        TempData["Error"] = "The role from currently logged user cannot be dropped!";
                    }
                    else
                    {
                        result = await userManager.RemoveFromRoleAsync(user, roleEdit.RoleName);
                    }
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }

        //GET /admin/roles/editrole
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                TempData["Error"] = $"Role with Id: {id} cannot be found";
                return NotFound();
            }

            var model = new RoleEdit
            {
                Id = role.Id,
                RoleName = role.Name
            };
            return View(model);
        }

        //POST /admin/roles/editrole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RoleEdit model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                TempData["Error"] = $"Role with Id: {model.Id} cannot be found";
                return NotFound();
            }
            else
            {
                if (role.Name == "admin" && model.RoleName != "admin")
                {
                    TempData["Error"] = $"admin role cannot be changed!";
                    return RedirectToAction("Index");
                }
                else
                {
                    role.Name = model.RoleName;

                    // Update the Role using UpdateAsync
                    var result = await roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = $"Role with Id: {model.Id} has been updated.";
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
        }
    }
}
