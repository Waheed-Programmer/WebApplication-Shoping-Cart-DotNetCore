using Microsoft.AspNetCore.Mvc;
using WelcomeWeb.Data;

namespace WelcomeWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public CategoryController(ApplicationDbContext Db)
        {
            _ApplicationDbContext = Db; 
        }
        public IActionResult Index()
        {
            try
            {
                var list = _ApplicationDbContext.Categories.ToList();
                return View(list);
            }
            catch (Exception)
            {

                return View();
            }
        }
    

    }
}
