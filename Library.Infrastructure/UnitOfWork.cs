using Library.Infrastructure.DatabaseContext;

namespace Library.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
