using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void PaymentStatus(int orderHeaderId, string SessionId, string PaymentIntentId)
        {
            var orderHeader = _context.orderHeaders.FirstOrDefault(x=>x.OrderHeaderId == orderHeaderId);
            orderHeader.DateOfPayment = DateTime.Now;
            orderHeader.PaymentIntentId = PaymentIntentId;
            orderHeader.SessionId = SessionId;
        }

        public void Update(OrderHeader orderHeader)
        {

            _context.orderHeaders.Update(orderHeader);          
            
        }

        public void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus = null)
        {
            var order = _context.orderHeaders.FirstOrDefault(x=>x.OrderHeaderId==orderHeaderId);
            if (order != null)
            {
                order.OrderStatus = orderStatus;
            }
            if(paymentStatus != null)
            {
                order.PaymentStatus = paymentStatus;
            }
        }
    }
}
