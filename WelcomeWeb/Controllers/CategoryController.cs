using Microsoft.AspNetCore.Mvc;

namespace WelcomeWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
