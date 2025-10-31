using System.Linq.Expressions;
using System.Reflection;


namespace Parcial3.Services.Interfaces
{
    public interface ICrudService<T>
    {
        void Delete(int id);
<<<<<<< HEAD
        void Update(T entity, string changeToValue, int inputOption);
=======
        void List();
>>>>>>> 5548ceba318c311045d9d03e630ae59d4db3bbcd
        T Search(int id);
        bool ShouldSkipPropertie(PropertyInfo property, bool allowLists = false);
        T SearchWhitIncludes(int id, params Expression<Func<T, object>>[] includes);
        List<PropertyInfo> ListModifyableProperties(T entity);
        IEnumerable<T> GetAll();
    }
}
