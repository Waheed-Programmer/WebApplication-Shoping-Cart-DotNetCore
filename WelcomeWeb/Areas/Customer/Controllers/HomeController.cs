
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

        public IActionResult Index()
        {
            IEnumerable<Product> Products = _unitofWork.Product.GetAll(includeProperties: "Category");
            return View(Products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
       
    }
}