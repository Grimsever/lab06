using System.Diagnostics;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab06.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InfoPage(InformationViewModel info)
        {
            return View(info);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}