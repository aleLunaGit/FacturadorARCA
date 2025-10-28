using System.Linq.Expressions;

namespace Parcial3.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetByID(int Id);
        T GetByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes);
        T GetByProperty(string propertyName, string value, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(int id);
        void Add(T entity);
    }
}
