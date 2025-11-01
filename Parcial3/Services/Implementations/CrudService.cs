using Parcial3.Modules;
using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Parcial3.Services.Implementations
{
    public class CrudService<T> : ICrudService<T> where T : class, new()
    {
        protected readonly IRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        public CrudService(IRepository<T> entity, IUnitOfWork unitOfWork)
        {
            _repository = entity;
            _unitOfWork = unitOfWork;
        }
        public void Delete(int id)
        {
                try
                {
                    _repository.Delete(id);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al eliminar {typeof(T).Name}.");
                }
        }
        public T Search(int id) => _repository.GetByID(id);
        public virtual T SearchWhitIncludes(int id, params Expression<Func<T, object>>[] includes)
        {
            return _repository.GetByIdWithIncludes(id, includes);

        }
        public virtual List<PropertyInfo> ListModifyableProperties(T entity)
        {
            if (entity == null) throw new Exception("La entidad no tiene una lista de propiedades");
            List<PropertyInfo> listProperties = new List<PropertyInfo>();
            var propertys = typeof(T).GetProperties();
            foreach (var item in propertys)
            {
                if (ShouldSkipPropertie(item)) continue;
                listProperties.Add(item);
            }
            return listProperties;
        }

        public bool ShouldSkipPropertie(PropertyInfo property, bool allowLists = false)
        {
            if (property.Name == "Id") return true;

            if (!allowLists && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return true;
            }

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string)) return true;

            if (property.Name == "ClientId" || property.Name == "InvoiceId") return true;


            return false;
        }
        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
