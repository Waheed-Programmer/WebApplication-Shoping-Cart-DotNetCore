using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; } = new Product();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
