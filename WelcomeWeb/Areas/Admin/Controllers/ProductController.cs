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
            Productvm.Products = _unitofWork.Product.GetAll();
           return View(Productvm);
        }
               
        [HttpGet]
        public IActionResult CreateUpdate(int? Id)
        {
            ProductVM cat = new ProductVM();
            if(Id==null || Id == 0)
            {
                return View(cat);
            }
            else
            {
                cat.Product = _unitofWork.Product.GetT(m => m.ProductId == Id);
                if (cat.Product == null)
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
        public IActionResult CreateUpdate(ProductVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (vm.Product.ProductId == 0)
                    {
                        _unitofWork.Product.Add(vm.Product);
                        TempData["save"] = "Data Save Successfully!";


                    }
                    else
                    {
                        _unitofWork.Product.Update(vm.Product);
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
            var c = _unitofWork.Product.GetT(m=>m.ProductId == Id);
            if (c == null)
            {
                return NotFound();

            }
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteData(int? Id)
        {           
                var c = _unitofWork.Product.GetT(m=>m.CategoryId==Id);

                if (c==null)
                {
                    return NotFound();
                }
                _unitofWork.Product.Delete(c);
                _unitofWork.Save();
                TempData["delete"] = "Data Delete Successfully!";
            return RedirectToAction("Index");           
           
        }

    }
}
