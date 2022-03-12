using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModel;

namespace WelcomeWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitofWork unitofWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork; 
            _webHostEnvironment = webHostEnvironment;
        }
        #region All products get throug json data
        public IActionResult AllProduct()
        {
            var products = _unitofWork.Product.GetAll();
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Index()
        {           
           return View();
        }
               
        [HttpGet]
        public IActionResult CreateUpdate(int? Id)
        {
            ProductVM cat = new ProductVM()
            {
                Product = new(),
                Categories = _unitofWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                    
                })
                
            };
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
        public IActionResult CreateUpdate(ProductVM vm , IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = string.Empty;
                    if (file != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImage");
                        fileName = Guid.NewGuid().ToString()+"-"+file.FileName;
                        string filePath = Path.Combine(uploadDir, fileName);
                        using(var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);    
                        }
                        vm.Product.ImgUrl = @"\ProductImage\" + fileName;
                    }
                    if (vm.Product.ProductId == 0)
                    {
                        _unitofWork.Product.Add(vm.Product);
                        TempData["save"] = "Product Save Successfully!";


                    }
                    else
                    {
                        _unitofWork.Product.Update(vm.Product);
                        TempData["edit"] = "Product Update Successfully!";


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
