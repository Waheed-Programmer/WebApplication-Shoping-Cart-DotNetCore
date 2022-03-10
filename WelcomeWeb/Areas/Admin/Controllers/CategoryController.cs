using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;

namespace WelcomeWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork; 
        }
        public IActionResult Index()
        {
           var list = _unitofWork.Category.GetAll();
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
                    _unitofWork.Category.Add(tbl);    
                    _unitofWork.Save();
                    TempData["save"] = "Data Save Successfully!";
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
            var c = _unitofWork.Category.GetT(m => m.CategoryId == Id);
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
                    _unitofWork.Category.Update(category);
                    _unitofWork.Save();
                    TempData["edit"] = "Data Update Successfully!";
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
            var c = _unitofWork.Category.GetT(m=>m.CategoryId == Id);
            if (c == null)
            {
                return NotFound();

            }
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteData(int? Id)
        {           
                var c = _unitofWork.Category.GetT(m=>m.CategoryId==Id);

                if (c==null)
                {
                    return NotFound();
                }
                _unitofWork.Category.Delete(c);
                _unitofWork.Save();
                TempData["delete"] = "Data Delete Successfully!";
            return RedirectToAction("Index");           
           
        }

    }
}
