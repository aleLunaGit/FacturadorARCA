using Parcial3.Modules;
using Parcial3.Repositories.Implementations;
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
        public void List()
        {
            List<T> allTheObjects = _repository.GetAll().ToList();
            var properties = typeof(T).GetProperties().ToList();
            foreach (T item in allTheObjects)
            {
                foreach (var property in properties)
                {
                    if (ShouldSkipPropertie(property, true)) continue;
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        continue;
                    }
                    Presentator.WriteLine($"{property.Name}: {property.GetValue(item)}.");
                }
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

        public void Update(T entity, string changeToValue, int inputOption)
        {
            if (entity == null || changeToValue == null || inputOption == null) return;
            var modifiablePropertys = ListModifyableProperties(entity);
            foreach (var prop in modifiablePropertys)
            {
                T entityFound = _repository.GetByProperty(prop.Name, changeToValue);
                if (entityFound != null)
                {
                    throw new Exception("Entidad encontrada con ese valor");
                }
            }
            var changeProperty = modifiablePropertys.ElementAt(inputOption - 1);
            var convertedValue = Convert.ChangeType(changeToValue, changeProperty.PropertyType);
            changeProperty.SetValue(entity, convertedValue);
            try
            {
                _repository.Update(entity);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar {typeof(T).Name}.");
            }
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
