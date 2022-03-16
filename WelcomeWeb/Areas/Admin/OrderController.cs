using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.CommonHelper;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModel;
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
        public IActionResult AllOrders(string status)
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
            switch (status)
            {
                case "pending":
                    orderHeaders = _unitofWork.OrderHeader.GetAll(x => x.PaymentStatus == PaymentStatus.StatusPending);
                    break;
                case "approved":
                    orderHeaders = _unitofWork.OrderHeader.GetAll(x => x.PaymentStatus == PaymentStatus.StatusApproved);
                    break;

                default:
                    orderHeaders = _unitofWork.OrderHeader.GetAll(includeProperties: "Applicationuser");
                    break;
            }
            return Json(new { data = orderHeaders });
        }
        #endregion



        public IActionResult OrderDetails(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                orderHeader =  _unitofWork.OrderHeader.GetT(x=>x.OrderHeaderId==id, includeProperties:"Applicationuser"),
                OrderDetail = _unitofWork.OrderDetail.GetAll(x=>x.Id==id, includeProperties:"Product")
            };
            return View();
        }
    }
}
