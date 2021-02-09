using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface IRepository <T> where T :class
    {
        Task <T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T,bool>> filter = null,
            string includeProperties = null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null
        );
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
        );
        void Insert(T entity);
        void RemoveById(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        void Update(T entity);
    }
}
