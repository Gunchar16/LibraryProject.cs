using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<T?> GetSingleOrDefaultAsync(int id);
        IQueryable<T> Query();
        Task Add(T entity);
        void Remove(T entity);
        void Update(T entity);

    }
}
