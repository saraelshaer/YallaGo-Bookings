using System.Linq.Expressions;

namespace YallaGo.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void HardDelete(T entity);
        void SoftDelete(T entity);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<U>> SelectAsync<U>(Expression<Func<T, U>> expression, Expression<Func<T, bool>> criteria = null);
        Task<bool> Exists(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria = null);
    }

}
