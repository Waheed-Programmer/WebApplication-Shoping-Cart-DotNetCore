using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var Productdb = _context.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (Productdb != null)
            {
                Productdb.ProductName = product.ProductName;
                Productdb.Description = product.Description;
                Productdb.Price = product.Price;
                if(product.ImgUrl != null)
                {
                    Productdb.ImgUrl = product.ImgUrl;
                }
            }
        }
    }
}
