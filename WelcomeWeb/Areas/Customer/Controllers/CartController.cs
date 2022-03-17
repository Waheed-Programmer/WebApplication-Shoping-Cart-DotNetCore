using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.CommonHelper;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModel;
using Stripe.Checkout;
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
                ListOfCart = _unitofWork.Cart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                orderHeader = new MyApp.Models.OrderHeader()
            };         
            foreach (var item in tCart.ListOfCart)
            {
                tCart.orderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            return View(tCart);
        }
        [Authorize]
        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            tCart = new CartVM()
            {
                ListOfCart = _unitofWork.Cart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                orderHeader = new MyApp.Models.OrderHeader()
            };
            tCart.orderHeader.Applicationuser = _unitofWork.Application.GetT(x => x.Id == claim.Value);
            tCart.orderHeader.Name = tCart.orderHeader.Applicationuser.Name;
            tCart.orderHeader.Address = tCart.orderHeader.Applicationuser.Address;
            tCart.orderHeader.Phon = tCart.orderHeader.Applicationuser.PhoneNumber;
            tCart.orderHeader.City = tCart.orderHeader.Applicationuser.City;
            tCart.orderHeader.PostalCode = tCart.orderHeader.Applicationuser.PinCode;
            tCart.orderHeader.State = tCart.orderHeader.Applicationuser.State;

            foreach (var item in tCart.ListOfCart)
            {
                tCart.orderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            return View(tCart);

        }
        [HttpPost]
        public IActionResult Summary(CartVM vm)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            tCart = new CartVM()
            {
                ListOfCart = _unitofWork.Cart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product"),
                orderHeader = new MyApp.Models.OrderHeader()
            };
            tCart.orderHeader.Applicationuser = _unitofWork.Application.GetT(x => x.Id == claim.Value);
            tCart.orderHeader.Name = tCart.orderHeader.Applicationuser.Name;
            tCart.orderHeader.Address = tCart.orderHeader.Applicationuser.Address;
            tCart.orderHeader.Phon = tCart.orderHeader.Applicationuser.PhoneNumber;
            tCart.orderHeader.City = tCart.orderHeader.Applicationuser.City;
            tCart.orderHeader.PostalCode = tCart.orderHeader.Applicationuser.PinCode;
            tCart.orderHeader.State = tCart.orderHeader.Applicationuser.State;
            tCart.orderHeader.OrderStatus = OrderStatus.StatusPending;
            tCart.orderHeader.PaymentStatus = PaymentStatus.StatusPending;
            tCart.orderHeader.DateOfOrder = DateTime.Now;
            foreach (var item in tCart.ListOfCart)
            {
                tCart.orderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            _unitofWork.OrderHeader.Add(tCart.orderHeader);
            _unitofWork.Save();
            foreach (var item in tCart.ListOfCart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = tCart.orderHeader.OrderHeaderId,
                    Count = item.Count,
                    Price = item.Product.Price

                };
                _unitofWork.OrderDetail.Add(orderDetail);
                _unitofWork.Save();
            }

            //Apply payment method
            var domain = "https://localhost:7207/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),            
                Mode = "payment",
                SuccessUrl = domain+$"customer/cart/OrderSuccess?id={tCart.orderHeader.OrderHeaderId}",
                CancelUrl = domain+$"customer/cart/index",
            };

            foreach (var item in tCart.ListOfCart)
            {

                var lieItemoptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price*100),
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

            _unitofWork.OrderHeader.PaymentStatus(tCart.orderHeader.OrderHeaderId, session.Id, session.PaymentIntentId);
            _unitofWork.Save();

            _unitofWork.Cart.DeleteRange(tCart.ListOfCart);
            _unitofWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


            
            return RedirectToAction("Index","Home");


        }

        public IActionResult OrderSuccess(int Id)
        {
            var order = _unitofWork.OrderHeader.GetT(x => x.OrderHeaderId == Id);
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            if(session.PaymentStatus.ToLower()== "paid")
            {
                _unitofWork.OrderHeader.UpdateStatus(Id, OrderStatus.StatusApproved, PaymentStatus.StatusApproved);
            }
            List<Cart> carts = _unitofWork.Cart.GetAll(x => x.ApplicationUserId == order.ApplicationuserId).ToList();
            _unitofWork.Cart.DeleteRange(carts);
            _unitofWork.Save();
            return View(Id);
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
