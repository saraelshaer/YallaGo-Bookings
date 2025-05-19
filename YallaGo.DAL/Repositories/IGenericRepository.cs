using System.Linq.Expressions;
using YallaGo.DAL.Consts;

namespace YallaGo.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync
        (
            Expression<Func<T, bool>> criteria = null,
            string[] includes = null,
            Expression<Func<T, object>> orderBy = null,
            OrderByDirection orderByDirection = OrderByDirection.Descending
         );

        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void HardDelete(T entity);
        void SoftDelete(T entity);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<bool> Exists(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria = null);
    }

}
