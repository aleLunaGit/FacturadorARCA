using Microsoft.EntityFrameworkCore;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using System.Linq.Expressions;

namespace Parcial3.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
        }
        public T GetByID(int Id)
        {

            return _dbSet.Find(Id);
        }
        public T GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, "Id");
            var equal = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return query.FirstOrDefault(lambda);
        }
        public T GetByProperty(string propertyName, string value, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, propertyName);
            var equal = Expression.Equal(property, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return query.FirstOrDefault(lambda);
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(int id)
        {
            _dbSet.Remove(_dbSet.Find(id));
        }
    }
}
