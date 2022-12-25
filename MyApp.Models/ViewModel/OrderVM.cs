using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models.ViewModel
{
    public class OrderVM
    {
        public OrderVM()
        {
            OrderDetail = new List<OrderDetail>();
            orderHeader = new OrderHeader();
        }
        public OrderHeader orderHeader { get; set; } 
        public IEnumerable<OrderDetail> OrderDetail { get; set; } 

    }
}
