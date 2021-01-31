namespace FootballLeague.Data.Contracts
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : class, new()
    {
        IQueryable<T> All(params Expression<Func<T, object>>[] includeExpressions);

        Task<T> GetByIdAsync(object id);

        T Add(T entity);

        bool Exists(T entity);

        void Delete(T entity);

        void ChangeState(T entity, EntityState state);

        Task<T> SaveAsync(T entity);

        Task SaveAsync();
    }
}
