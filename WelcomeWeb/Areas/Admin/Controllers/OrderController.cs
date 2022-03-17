using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.CommonHelper;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModel;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace WelcomeWeb.Areas.Admin.Controllers
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
                orderHeaders = _unitofWork.OrderHeader.GetAll(x => x.ApplicationuserId == claim.Value);

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


        [HttpGet]
        public IActionResult OrderDetails(int OrderID)
        {

            OrderVM orderVM = new OrderVM()
            {
                orderHeader = _unitofWork.OrderHeader.GetT(x => x.OrderHeaderId == OrderID, includeProperties: "Applicationuser"),
                OrderDetail = _unitofWork.OrderDetail.GetAll(x => x.Id == OrderID, includeProperties: "Product")
            };
            return View(orderVM);

        }

        [HttpPost]
        [Authorize(Roles=WebsiteRole.Role_Admin+","+WebsiteRole.Role_Employee)]
        public IActionResult OrderDetails(OrderVM vM)
        {
            var orderHeader = _unitofWork.OrderHeader.GetT(x => x.OrderHeaderId == vM.orderHeader.OrderHeaderId);
            orderHeader.Name = vM.orderHeader.Name;
            orderHeader.Address = vM.orderHeader.Address;
            orderHeader.Phon =  vM.orderHeader.Phon;
            orderHeader.City = vM.orderHeader.City;
            orderHeader.State = vM.orderHeader.State;
            orderHeader.PostalCode = vM.orderHeader.PostalCode;
            if(vM.orderHeader.Carrior != null)
            {
                orderHeader.Carrior = vM.orderHeader.Carrior;
            }
            if(vM.orderHeader.TrackingNumber != null)
            {
                orderHeader.TrackingNumber = vM.orderHeader.TrackingNumber;
            }
            _unitofWork.OrderHeader.Update(orderHeader);
            _unitofWork.Save();
            TempData["Save"] = "Information Updated";
            return RedirectToAction("OrderDetails", "Order", new { id = vM.orderHeader.OrderHeaderId });
        }


        [Authorize(Roles = WebsiteRole.Role_Admin + "," + WebsiteRole.Role_Employee)]
        public IActionResult InProcess(OrderVM vM)
        {
            _unitofWork.OrderHeader.UpdateStatus(vM.orderHeader.OrderHeaderId, OrderStatus.StatusInProcess);
            _unitofWork.Save();
            TempData["Save"] = "Order status Updated-Inprocess";
            return RedirectToAction("OrderDetails", "Order", new { id = vM.orderHeader.OrderHeaderId });

        }

        [Authorize(Roles = WebsiteRole.Role_Admin + "," + WebsiteRole.Role_Employee)]
        public IActionResult Shipped(OrderVM vM)
        {
            var orderHeader = _unitofWork.OrderHeader.GetT(x => x.OrderHeaderId == vM.orderHeader.OrderHeaderId);
            orderHeader.Carrior = vM.orderHeader.Carrior;
            orderHeader.TrackingNumber = vM.orderHeader.TrackingNumber;
            orderHeader.OrderStatus = OrderStatus.StatusShipped;
            orderHeader.DateOfShipping = DateTime.Now;

            _unitofWork.OrderHeader.Update(orderHeader);
            _unitofWork.Save();
            TempData["Save"] = "Order status Updated-Shipped";
            return RedirectToAction("OrderDetails", "Order", new { id = vM.orderHeader.OrderHeaderId });

        }

        [Authorize(Roles = WebsiteRole.Role_Admin + "," + WebsiteRole.Role_Employee)]
        public IActionResult CancelOrder(OrderVM vM)
        {
            var orderHeader = _unitofWork.OrderHeader.GetT(x => x.OrderHeaderId == vM.orderHeader.OrderHeaderId);
            if (orderHeader.PaymentStatus == PaymentStatus.StatusApproved)
            {
                var refund = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };
                var service = new RefundService();
                Refund Refund = service.Create(refund);
                _unitofWork.OrderHeader.UpdateStatus(vM.orderHeader.OrderHeaderId, OrderStatus.StatusCancelled, OrderStatus.StatusRefunded);


            }
            else
            {
                _unitofWork.OrderHeader.UpdateStatus(vM.orderHeader.OrderHeaderId, OrderStatus.StatusCancelled, OrderStatus.StatusCancelled);

            }
            _unitofWork.Save();
            TempData["Save"] = "Order Cancelled";
            return RedirectToAction("OrderDetails", "Order", new { id = vM.orderHeader.OrderHeaderId });

        }
        
        public IActionResult PayNow(OrderVM vm)
        {

            var orderHeader = _unitofWork.OrderHeader.GetT(x => x.OrderHeaderId == vm.orderHeader.OrderHeaderId, includeProperties: "Applicationuser");
            var OrderDetail = _unitofWork.OrderDetail.GetAll(x => x.Id == vm.orderHeader.OrderHeaderId, includeProperties: "Product");

            //Apply payment method
            var domain = "https://localhost:7207/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderSuccess?id={vm.orderHeader.OrderHeaderId}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in OrderDetail)
            {

                var lieItemoptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ProductName,
                        },


                    },
                    Quantity = item.Count,


                };
                options.LineItems.Add(lieItemoptions);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitofWork.OrderHeader.PaymentStatus(vm.orderHeader.OrderHeaderId, session.Id, session.PaymentIntentId);
            _unitofWork.Save();
            
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            return RedirectToAction("Index", "Home");
        }


    }
}
