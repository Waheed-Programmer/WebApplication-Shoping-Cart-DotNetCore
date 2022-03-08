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
            var list = Context.Categories.ToList();
           return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
             
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Context.Categories.Add(tbl);    
                    Context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                return View(tbl);
            }
            return View();
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
