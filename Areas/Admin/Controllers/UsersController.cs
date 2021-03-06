﻿using System;
using System.Collections.Generic;
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
    public class UsersController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
        }

        //GET /admin/users
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        //GET /admin/users/register
        public IActionResult Register() => View();

        //POST /admin/users/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                Random rnd = new Random();
                AppUser appUser = new AppUser
                {
                    Name = user.Name,
                    UserName = rnd.Next(100, 999) + user.Name.Substring(0, 2) + rnd.Next(10, 99),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["Success"] = "The user " + appUser.UserName + " has been added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }

        //GET /admin/users/edit/id
        public async Task<IActionResult> Edit(string id)
        {
            _ = new AppUser();
            AppUser appUser = await userManager.FindByIdAsync(id);

            UserEditDetail user = new UserEditDetail(appUser);

            return View(user);
        }

        //POST /admin/users/edit/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditDetail user)
        {
            AppUser appUser = new AppUser();
            appUser = await userManager.FindByIdAsync(user.Id);

            if (ModelState.IsValid)
            { 
                appUser.Name = user.Name;
                appUser.Email = user.Email;
                appUser.PhoneNumber = user.PhoneNumber;
                appUser.Address = user.Address;

                if (user.Password != null)
                {
                    appUser.PasswordHash = passwordHasher.HashPassword(appUser, user.Password);
                }

                IdentityResult result = await userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "User information has been updated!";
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
            var user = await userManager.FindByIdAsync(id);

            if (user.UserName == "superadmin")
            {
                TempData["Error"] = "superadmin cannot be dropped!";
                return RedirectToAction("Index");
            }
            else
            {
                if (user.UserName == appUser.UserName)
                {
                    TempData["Error"] = "Sorry you cannot drop yourself!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "User Successfully Deleted";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Error Deleting User";
                        return RedirectToAction("Index");
                    }
                }
            }
        }
    }
}
