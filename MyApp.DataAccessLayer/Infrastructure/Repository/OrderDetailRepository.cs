using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.orderDetails.Update(orderDetail);
            //var Categorydb = _context.Categories.FirstOrDefault(x=>x.CategoryId == category.CategoryId);
            //if(Categorydb != null)
            //{
            //    Categorydb.CategoryName = category.CategoryName;
            //    Categorydb.OrderPlace = category.OrderPlace;
            //}
            
        }
    }
}
