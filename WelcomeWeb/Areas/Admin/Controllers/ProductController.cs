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
            var products = _unitofWork.Product.GetAll(includeProperties:"Category");
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Index()
        {           
           return View();
        }
               
        [HttpGet]
        public IActionResult CreateUpdate(int? id)
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
            if(id==null || id == 0)
            {
                return View(cat);
            }
            else
            {
                cat.Product = _unitofWork.Product.GetT(m => m.ProductId == id);
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
                        if(vm.Product.ImgUrl != null)
                        {
                            var OldpathImage = Path.Combine(_webHostEnvironment.WebRootPath, vm.Product.ImgUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(OldpathImage))
                            {
                                System.IO.File.Delete(OldpathImage);
                            }
                        }
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
            return View(vm);   
        }

        
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {           
            var c = _unitofWork.Product.GetT(m=>m.CategoryId==Id);

            if (c==null)
            {
                return Json(new {success =false, message="Data has not been Fetch"});
             }
            else
            {
            var OldpathImage = Path.Combine(_webHostEnvironment.WebRootPath, c.ImgUrl.TrimStart('\\'));
            if (System.IO.File.Exists(OldpathImage))
            {
                System.IO.File.Delete(OldpathImage);
            }
            _unitofWork.Product.Delete(c);
            _unitofWork.Save();
            return Json(new { success = true, message = "Data has been Deleted" });

        }



        }

    }
}
