
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System.Diagnostics;
using System.Security.Claims;

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

        [HttpGet]
        public IActionResult Detail(int? ProductId)
        {
            Cart cart = new Cart()
            {
             Product = _unitofWork.Product.GetT(x => x.ProductId == ProductId, includeProperties: "Category"),
             Count = 1,
             ProductId = (int)ProductId
            };
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  
        [Authorize]
        public IActionResult Detail(Cart cart)
        {

            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claim.Value;
                var cartItem = _unitofWork.Cart.GetT(x => x.ProductId == cart.ProductId && x.ApplicationUserId == claim.Value);
                if (cartItem == null)
                {
                    _unitofWork.Cart.Add(cart);
                    _unitofWork.Save();
                    HttpContext.Session.SetInt32("SessionCart",_unitofWork
                        .Cart.GetAll(x=>x.ApplicationUserId==claim.Value).ToList().Count);
                }
                else
                {
                    _unitofWork.Cart.IncreamentCartItem(cartItem, cart.Count);
                    _unitofWork.Save();

                }
            }
            
            return RedirectToAction("Index");
        }

       
       
    }
}