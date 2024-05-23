using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Giftos.Controllers
{
    public class HomeController : Controller
    {
 
        public IActionResult Index()
        {
            return View();
        }

    }
}
