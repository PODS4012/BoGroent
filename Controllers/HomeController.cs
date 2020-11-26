using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoGroent.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoGroent.Controllers
{
    public class HomeController : Controller
    {
        private readonly BoGroentContext contex;
        public HomeController(BoGroentContext contex)
        {
            this.contex = contex;
        }

        //GET /admin/cars
        public async Task<IActionResult> Index()
        {
            return View(await contex.Cars.OrderBy(x => x.Id).ToListAsync());
        }
    }
}
