using System.Linq.Expressions;
using System.Reflection;


namespace Parcial3.Services.Interfaces
{
    public interface ICrudService<T>
    {
        void Delete(int id);
        void Update(T entity, string changeToValue, int inputOption);
        void List();
        T Search(int id);
        bool ShouldSkipPropertie(PropertyInfo property, bool allowLists = false);
        T SearchWhitIncludes(int id, params Expression<Func<T, object>>[] includes);
        List<PropertyInfo> ListModifyableProperties(T entity);
        IEnumerable<T> GetAll();
    }
}
