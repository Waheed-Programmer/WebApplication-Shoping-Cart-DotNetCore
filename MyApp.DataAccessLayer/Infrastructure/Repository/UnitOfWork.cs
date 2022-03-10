using MyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get;private set; }
        public IProductRepository Product { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);            
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
