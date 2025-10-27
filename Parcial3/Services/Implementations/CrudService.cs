using Parcial3.Repositories.Interfaces;
using Parcial3.Services.Interfaces;
using Parcial3.UI;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Parcial3.Services.Implementations
{
    public class CrudService<T> : ICrudService<T> where T : class, new()
    {
        protected readonly IRepository<T> _repository;
        public CrudService(IRepository<T> entity)
        {
            _repository = entity;
        }
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
        public void List()
        {
            List<T> allTheObjects = _repository.GetAll().ToList();
            var properties = typeof(T).GetProperties().ToList();
            foreach (T item in allTheObjects)
            {
                    Presentator.WriteLine("---------------------------------------------");
                foreach (var property in properties)
                {
                if(ShouldSkipPropertie(property, true)) continue;
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) { 
                        continue;
                    }
                Presentator.WriteLine($"{property.Name}: {property.GetValue(item)}.");
                }
            }
        }
        public T Search(int id) => _repository.GetByID(id);

        public virtual T SearchWhitIncludes(int id, params Expression<Func<T, object>>[] includes)
        {
            // TODO: Arreglar el principio SRP del Search
            return _repository.GetByIdWithIncludes(id, includes);
            
        }
        public virtual List<PropertyInfo> ListModifyableProperties(T entity)
        {
            if (entity == null) throw new Exception("La entidad no tiene una lista de propiedades");
            List<PropertyInfo> listProperties = new List<PropertyInfo> ();
                var propertys= typeof(T).GetProperties();
            foreach (var item in propertys)
            {
                if (ShouldSkipPropertie(item)) continue;
                listProperties.Add(item);
            }
            return listProperties;
        }

        public virtual void ConvertValues(T entity, List<string> convertThisValues)
        {
            var Properties = typeof(T).GetProperties();
            var listProperties = ListModifyableProperties(entity);
            int count = 0;
            foreach (var convertValue in convertThisValues)
            {
                var property = listProperties.ElementAt(count);
                try
                {
                    var correctedTypeValue = Convert.ChangeType(convertValue, property.PropertyType);
                    property.SetValue(entity, correctedTypeValue);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
                count++;
            }
        }
        public virtual void Register(T entity)
        {
            _repository.Add(entity);
        }
        public virtual void Register(T entity, List<string> listOfInputs)
        {
            ConvertValues(entity, listOfInputs);
            _repository.Add(entity);
        }
        public void Update(T entity, string changeToValue, int inputOption)
        {
            if(entity == null || changeToValue == null || inputOption == null) return;
            var modifiablePropertys = ListModifyableProperties(entity);
            var changeProperty =  modifiablePropertys.ElementAt(inputOption - 1);
            // Cambiamos los tipos para que coincidan con los pedidos por la propiedad en cuestion
            var convertedValue = Convert.ChangeType(changeToValue, changeProperty.PropertyType);
            // Setteamos el valor cambiado a la propiedad
            changeProperty.SetValue(entity, convertedValue);
            _repository.Update(entity);
        }
        public bool ShouldSkipPropertie(PropertyInfo property, bool allowLists = false)
        {
            // Ignora la propiedad "Id" porque es generada por la base de datos
            if (property.Name == "Id") return true;

            // Ignora propiedades que son listas (relaciones de uno a muchos)
            if (!allowLists && property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return true;
            }

            // Ignora propiedades que son otras clases (relaciones de uno a uno)
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string)) return true;

            // Ignora propiedades que no queremos que el usuario ingrese manualmente
            if (property.Name == "ClientId" || property.Name == "InvoiceId") return true;


            return false;
        }
        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
