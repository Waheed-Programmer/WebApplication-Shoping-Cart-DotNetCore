using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System.Security.Claims;

namespace WelcomeWeb.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public OrderController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region All Order get throug json data
        public IActionResult AllOrders()
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                orderHeaders = _unitofWork.OrderHeader.GetAll(includeProperties: "Applicationuser");

            }
            else
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitofWork.OrderHeader.GetAll(x=>x.ApplicationuserId==claim.Value);

            }
            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
