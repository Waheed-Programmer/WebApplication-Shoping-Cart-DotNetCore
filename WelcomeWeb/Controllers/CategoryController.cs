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
        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if(Id==null || Id == 0)
            {
                return NotFound();
            }
            var c = Context.Categories.Find(Id);
            if (c == null)
            {
                return NotFound();

            }
            return View(c);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Context.Categories.Update(category);
                    Context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                return View(category);
            }
            return RedirectToAction("Index");   
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var c = Context.Categories.Find(Id);
            if (c == null)
            {
                return NotFound();

            }
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteData(int? Id)
        {
           
                var c = Context.Categories.Find(Id);

                if (c==null)
                {
                    return NotFound();
                }
                Context.Categories.Remove(c);
                Context.SaveChanges();
                return RedirectToAction("Index");           
           
        }

    }
}
