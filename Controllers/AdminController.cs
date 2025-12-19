using Microsoft.AspNetCore.Mvc;

namespace Kargar.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
