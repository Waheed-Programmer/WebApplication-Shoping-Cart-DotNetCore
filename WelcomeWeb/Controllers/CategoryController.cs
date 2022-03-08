using Microsoft.AspNetCore.Mvc;
using WelcomeWeb.Data;
using WelcomeWeb.Master;
using WelcomeWeb.Models;

namespace WelcomeWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext Context;
        public CategoryController(ApplicationDbContext Db)
        {
            Context = Db; 
        }
        public IActionResult Index()
        {
           return View();
        }
        public JsonResult GetAll()
        {
            List<Category> _list = new CategoryDA(Context).GetAll(); 
            return Json(_list);
        }
        public JsonResult GetById(int Id)
        {
            Category _list = new CategoryDA(Context).GetbyId(Id);
            return Json(_list);
        }
        public JsonResult Remove(string Id)
        {
            Category _list = new CategoryDA(Context).Remove(Convert.ToInt32(Id));
            return Json(_list);
        }

        public JsonResult Save(Category category)
        {
            Category _list = new CategoryDA(Context).Save(category);
            return Json(_list); 
        }

    }
}
