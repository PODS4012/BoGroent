using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoGroent.Infrastructure;
using BoGroent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoGroent.Controllers
{
    [Authorize]
    public class PersonalLeasesController : Controller
    {
        private readonly BoGroentContext contex;
        private readonly UserManager<AppUser> userManager;
        public PersonalLeasesController(BoGroentContext contex, UserManager<AppUser> userManager)
        {
            this.contex = contex;
            this.userManager = userManager;
        }

        //GET /admin/leases
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            return View(await contex.Leases.Where(x => x.UserId == appUser.Id).OrderByDescending(x => x.Id).ToListAsync());
        }
    }
}
