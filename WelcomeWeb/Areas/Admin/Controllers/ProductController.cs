using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModel;

namespace WelcomeWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public ProductController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork; 
        }
        public IActionResult Index()
        {
            ProductVM Productvm = new ProductVM();
            Productvm.products = _unitofWork.Product.GetAll();
           return View(Productvm);
        }
               
        [HttpGet]
        public IActionResult CreateUpdate(int? Id)
        {
            CategoryVM cat = new CategoryVM();
            if(Id==null || Id == 0)
            {
                return View(cat);
            }
            else
            {
                cat.Category = _unitofWork.Category.GetT(m => m.CategoryId == Id);
                if (cat.Category == null)
                {
                    return NotFound();

                }
                else
                {
                    return View(cat);
                }
            }           
        }

        [HttpPost]
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (vm.Category.CategoryId == 0)
                    {
                        _unitofWork.Category.Add(vm.Category);
                        TempData["save"] = "Data Save Successfully!";


                    }
                    else
                    {
                        _unitofWork.Category.Update(vm.Category);
                        TempData["edit"] = "Data Update Successfully!";


                    }
                    _unitofWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                return View(vm);
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
