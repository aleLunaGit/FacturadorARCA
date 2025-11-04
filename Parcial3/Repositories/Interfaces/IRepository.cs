using System.Linq.Expressions;

namespace Parcial3.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(int id);
        IEnumerable<T> GetAll();
        T GetByID(int Id);
        T GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes);
        T GetByProperty(string propertyName, string value, params Expression<Func<T, object>>[] includes)
        void Update(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}