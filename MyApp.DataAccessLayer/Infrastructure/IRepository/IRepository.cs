using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.IRepository
{
    public interface IRepository<T> where T: class
    {
        //this interface is show that all the generics reused method 
        IEnumerable<T> GetAll();
        T GetT(Expression<Func<T, bool>> predicate);
        void add(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
