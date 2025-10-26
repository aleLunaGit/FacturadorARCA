using System.Linq.Expressions;
using System.Reflection;


namespace Parcial3.Interfaces
{
    public interface ICrudService<T>
    {
        void Delete(int id);
        void Update(T entity, string changeToValue, int inputOption);
        void List();
        bool ShouldSkipPropertie(PropertyInfo property, bool allowLists = false);
        public IEnumerable<T> GetAll();
    }
    public interface IInvoiceService<T> {
        void Register(T entity);
        void Search(int id, params Expression<Func<T, object>>[] includes);
    }

}
