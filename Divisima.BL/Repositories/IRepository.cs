using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Divisima.BL.Repositories
{
    public interface IRepository<T>
    {
        public IQueryable<T> GetAll();//IQueryable, Icollection, IList,List: IEnumerable
        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp);
        public T GetBy(Expression<Func<T, bool>> exp);
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Update(T entity, params Expression<Func<T, object>>[] expressions);
        public Task Delete(T entity);
        public Task DeleteRange(IEnumerable<T> entities);
    }
}
//CRUD