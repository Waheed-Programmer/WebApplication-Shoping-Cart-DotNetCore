
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using MyApp.Models;
using System.Diagnostics;

namespace WelcomeWeb.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, UnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> Products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(Products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
       
    }
}