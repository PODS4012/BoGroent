using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoGroent.Infrastructure;
using BoGroent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BoGroent.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class LeasesController : Controller
    {
        private readonly BoGroentContext contex;
        private readonly UserManager<AppUser> userManager;
        //private readonly IDataProtector dataProtector; 
        public LeasesController(BoGroentContext contex, UserManager<AppUser> userManager)
        {
            this.contex = contex;
            this.userManager = userManager;
            //this.dataProtector = dataProtector.CreateProtector("decryptorSecretKey");
        }

        //GET /admin/leases
        public async Task<IActionResult> Index()
        {
            return View(await contex.Leases.OrderByDescending(x => x.Id).ToListAsync());
        }

        //GET /admin/leases/create
        public IActionResult Create() 
        {
            ViewBag.CarId = new SelectList(contex.Cars.OrderBy(x => x.Id), "Id", "Id" );
            ViewBag.UserId = new SelectList(contex.Users.OrderBy(x => x.Id), "Id" , "UserName");
           return View(); 
        }

        //POST /admin/leases/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lease lease)
        {
            ViewBag.CarId = new SelectList(contex.Cars.OrderBy(x => x.Id), "Id", "Id");
            ViewBag.UserId = new SelectList(contex.Users.OrderBy(x => x.Id), "Id", "UserName");

            if (ModelState.IsValid)
            {
                var user = await contex.Users.FirstOrDefaultAsync(x => x.Id == lease.UserUserName);
                var car = await contex.Cars.FirstOrDefaultAsync(x => x.Id == lease.CarId);

                if (lease.StartDate > lease.EndDate)
                {
                    ModelState.AddModelError("", "The lease End Date has to be larger than Start date");
                    return View(lease);
                }
                if (lease.StartDate == lease.EndDate)
                {
                    ModelState.AddModelError("", "Minimum lease time is 1 day");
                    return View(lease);
                }

                var leasesCount = contex.Leases.Where(x => x.CarId == lease.CarId && x.EndDate >= lease.StartDate && x.StartDate <= lease.EndDate);

                if (leasesCount.Count() > 0)
                {
                    var leaseLast = await contex.Leases.Where(x => x.CarId == lease.CarId).MaxAsync(x => x.EndDate);

                    ModelState.AddModelError("", $"{car.Brand} is already booked in that period. Next free booking slot is avaiable on: {leaseLast.Date.AddDays(1):dd-MM-yyyy}");
                    return View(lease);
                }


                lease.CarBrand = car.Brand;
                lease.CarColor = car.Color;
                lease.CarRentPrice = car.RentPrice;
                lease.UserId = user.Id;
                lease.UserUserName = user.UserName;
                lease.UserName = user.Name;
                lease.Payment = "Pending";

                contex.Add(lease);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The lease has been created!";

                return RedirectToAction("Index");
            }

            return View(lease);
        }

        //GET /admin/leases/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.CarId = new SelectList(contex.Cars.OrderBy(x => x.Id), "Id", "Id");
            ViewBag.UserId = new SelectList(contex.Users.OrderBy(x => x.Id), "UserName", "UserName");

            Lease lease = await contex.Leases.FindAsync(id);

            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        //POST /admin/leases/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lease lease)
        {
            ViewBag.CarId = new SelectList(contex.Cars.OrderBy(x => x.Id), "Id", "Id");
            ViewBag.UserId = new SelectList(contex.Users.OrderBy(x => x.Id), "UserName", "UserName");

            if (ModelState.IsValid)
            {
                var user = await contex.Users.FirstOrDefaultAsync(x => x.UserName == lease.UserUserName);
                var car = await contex.Cars.FirstOrDefaultAsync(x => x.Id == lease.CarId);

                if (lease.StartDate > lease.EndDate)
                {
                    ModelState.AddModelError("", "The lease End Date has to be larger than Start date");
                    return View(lease);
                }
                if (lease.StartDate == lease.EndDate)
                {
                    ModelState.AddModelError("", "Minimum lease time is 1 day");
                    return View(lease);
                }
                if (lease.Payment == null)
                {
                    ModelState.AddModelError("", "Select payment option");
                    return View(lease);
                }

                lease.CarBrand = car.Brand;
                lease.CarColor = car.Color;
                lease.CarRentPrice = car.RentPrice;
                lease.UserId = user.Id;
                lease.UserUserName = user.UserName;
                lease.UserName = user.Name;

                contex.Update(lease);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The lease has been updated!";

                return RedirectToAction("Index");
            }

            return View(lease);
        }

        //GET /admin/leases/edit/id
        public async Task<IActionResult> Delete(int id)
        {
            Lease lease = await contex.Leases.FindAsync(id);

            if (lease == null)
            {
                TempData["Error"] = "The lease does not exist!";
            }
            else
            {
                contex.Leases.Remove(lease);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The lease has been deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}
