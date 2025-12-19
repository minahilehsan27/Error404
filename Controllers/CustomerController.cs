using Microsoft.AspNetCore.Mvc;

namespace Kargar.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
