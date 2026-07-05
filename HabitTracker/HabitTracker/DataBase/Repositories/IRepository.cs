using System.Linq.Expressions;

namespace HabitTracker.DataBase.Repositories
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllByFuncAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetOneByFuncAsync(Expression<Func<T, bool>> predicate);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
