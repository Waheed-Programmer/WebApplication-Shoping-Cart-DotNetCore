using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models.ViewModel
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; } = new OrderHeader();
        public IEnumerable<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    }
}
