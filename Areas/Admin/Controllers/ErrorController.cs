using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BoGroent.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "That page does not exists.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Oops, there was a problem.";
                    break;
            }
            return View("Not Found");
        }
    }
}
