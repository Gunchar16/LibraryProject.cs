using Library.Infrastructure.DatabaseContext;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class, IPrimaryKey
    {
        protected readonly DataContext _context;

        public GenericRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<T?> GetSingleOrDefaultAsync(int id)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }

}
