using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models.ViewModel;
using System.Security.Claims;

namespace WelcomeWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CartVM tCart { get; set; }
        public CartController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            tCart = new CartVM()
            {
                ListOfCart = _unitofWork.Cart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            foreach (var item in tCart.ListOfCart)
            {
                tCart.Total += (item.Product.Price * item.Count);
            }
            return View(tCart);
        }

        public IActionResult Plus(int id)
        {
            var cart = _unitofWork.Cart.GetT(x=>x.Id == id);
            _unitofWork.Cart.IncreamentCartItem(cart, 1);
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int id)
        {
            var cart = _unitofWork.Cart.GetT(x => x.Id == id);
            if (cart.Count <= 1)
            {
                _unitofWork.Cart.Delete(cart);
            }
            else
            {
               _unitofWork.Cart.DecrementCartItem(cart, 1);

            }
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var cart = _unitofWork.Cart.GetT(x => x.Id == id);
            _unitofWork.Cart.Delete(cart);
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
