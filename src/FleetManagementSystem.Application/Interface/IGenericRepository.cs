using System.Linq.Expressions;

namespace FleetManagementSystem.Application.Interface;

public interface IGenericRepository<T> where T : class
{
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        void Delete(T entity);
       Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);} 