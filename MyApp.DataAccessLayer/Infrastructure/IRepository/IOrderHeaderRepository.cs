using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int orderHeaderId, string orderStatus, string? paymentStatus=null);
        void PaymentStatus(int orderHeaderId, string SessionId, string PaymentIntentId);

    }
}
