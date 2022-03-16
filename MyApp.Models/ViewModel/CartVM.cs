using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models.ViewModel
{
    public class CartVM
    {
        public IEnumerable<Cart> ListOfCart { get; set; } = new List<Cart>();
        public OrderHeader orderHeader { get; set; } = new OrderHeader();
    }
}
