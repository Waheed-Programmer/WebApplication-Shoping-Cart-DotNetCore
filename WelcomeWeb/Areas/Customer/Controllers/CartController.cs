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

        public CartController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            CartVM tCart = new CartVM()
            {
                ListOfCart = _unitofWork.Cart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            return View(tCart);
        }
    }
}
