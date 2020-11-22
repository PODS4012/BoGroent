using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoGroent.Infrastructure;
using BoGroent.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoGroent.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarsController : Controller
    {
        private readonly BoGroentContext contex;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CarsController(BoGroentContext contex, IWebHostEnvironment webHostEnvironment)
        {
            this.contex = contex;
            this.webHostEnvironment = webHostEnvironment;
        }

        //GET /admin/cars
        public async Task<IActionResult> Index()
        {
            return View(await contex.Cars.OrderByDescending(x => x.Id).ToListAsync());
        }

        //GET /admin/cars/details/3
        public async Task<IActionResult> Details(int id)
        {
            Car car = await contex.Cars.FirstOrDefaultAsync(x => x.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        //GET /admin/cars/create
        public IActionResult Create() => View();

        //POST /admin/cars/create
        [HttpPost]
        [ValidateAntiForgeryToken] //protect against CSRF attacks 
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                var carId = await contex.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);
                if (carId != null)
                {
                    ModelState.AddModelError("", "The Car already exists.");
                    return View(car);
                }

                string imageName = "noimagecar.png";
                if (car.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/cars");
                    imageName = Guid.NewGuid().ToString() + "_" + car.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await car.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }
                car.Image = imageName;

                contex.Add(car);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The car has been added!";

                return RedirectToAction("Index");
            }

            return View(car);
        }

        //GET /admin/cars/edit/3
        public async Task<IActionResult> Edit(int id)
        {
            Car car = await contex.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        //POST /admin/cars/edit/3
        [HttpPost]
        [ValidateAntiForgeryToken] //protect against CSRF attacks 
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (ModelState.IsValid)
            {
                var carId = await contex.Cars.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Id == car.Id);
                if (carId != null)
                {
                    ModelState.AddModelError("", "The car already exists.");
                    return View(car);
                }

                if (car.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/cars");

                    if (!string.Equals(car.Image, "noimagecar.png") && car.Image != null)
                    {
                        string oldImagePath = Path.Combine(uploadsDir, car.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + car.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await car.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    car.Image = imageName;
                }


                contex.Update(car);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The car has been updated!";

                return RedirectToAction("Index");
            }

            return View(car);
        }

        //GET /admin/cars/delete/3
        public async Task<IActionResult> Delete(int id)
        {
            Car car = await contex.Cars.FindAsync(id);

            if (car == null)
            {
                TempData["Error"] = "The car does not exist!";
            }
            else
            {
                if (!string.Equals(car.Image, "noimage.png") && car.Image != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/cars");
                    string oldImagePath = Path.Combine(uploadsDir, car.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                contex.Cars.Remove(car);
                await contex.SaveChangesAsync();

                TempData["Success"] = "The car has been deleted!";
            }

            return RedirectToAction("Index");
        }

    }
}
