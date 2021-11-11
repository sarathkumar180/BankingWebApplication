using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BankingWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace BankingWebApplication.Controllers
{

    public class HomeController : Controller
    {
 
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("CustomerNo") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Customers");
            }
        }

            
        public IActionResult About()
        {
            if (HttpContext.Session.GetString("CustomerNo") != null)
            {
                ViewData["Message"] = "Your application description page.";
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Customers");
            }
       


        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
