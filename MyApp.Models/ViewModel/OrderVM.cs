using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models.ViewModel
{
    public class OrderVM
    {

        public virtual IEnumerable<OrderDetail> OrderDetail { get; set; }
        public OrderHeader orderHeader { get; set; } 
        

    }
}
