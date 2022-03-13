
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System.Diagnostics;

namespace WelcomeWeb.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _unitofWork;

        public HomeController(ILogger<HomeController> logger, IUnitofWork unitOfWork)
        {
            _logger = logger;
            _unitofWork = unitOfWork;   
           
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> Products = _unitofWork.Product.GetAll(includeProperties: "Category");
            return View(Products);
        }

        public IActionResult Detail(int? id)
        {
            Product Product = _unitofWork.Product.GetT(x=>x.ProductId==id,includeProperties: "Category");
            return View(Product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
       
    }
}