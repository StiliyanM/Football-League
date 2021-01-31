namespace FootballLeague.Data.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using FootballLeague.Data.Contracts;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly DbContext context;
        private readonly DbSet<T> entities;

        public Repository(
            FootballLeagueContext context
        )
        {
            this.context = context;
            this.entities = context.Set<T>();
        }

        public IQueryable<T> All(params Expression<Func<T, object>>[] includeExpressions)
        {
            IQueryable<T> set = this.entities;

            foreach (var includeExpression in includeExpressions)
            {
                set = set.Include(includeExpression);
            }

            return set;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var entity = await this.entities.FindAsync(id);

            return entity;
        }

        public T Add(T entity)
        {
            return this.entities.Add(entity).Entity;
        }

        public bool Exists(T entity)
    => this.entities.Contains(entity);

        public void Delete(T entity)
        {
            this.entities.Remove(entity);
        }

        public void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.entities.Attach(entity);
            }

            entry.State = state;
        }

        public async Task<T> SaveAsync(T entity)
        {
            if (this.Exists(entity))
            {
                this.ChangeState(entity, EntityState.Modified);
            }
            else
            {
                this.entities.Add(entity);
            }

            await this.SaveAsync();
            return entity;
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
