using Parcial3.Interfaces;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Parcial3.Modules
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
        public virtual void Search(int id, params Expression<Func<T, object>>[] includes)
        {
            //
            var entity = _repository.GetByIdWithIncludes(id, includes);
            Presentator.WriteLine($"\n--- Detalles de {typeof(T).Name} (ID: {id}) ---");
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // Si no son tipo lista, mostrar los datos de las propiedades
                if (property.PropertyType.IsGenericType &&
                    property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) )
                {
                    
                    var itemList = property.GetValue(entity) as IEnumerable;
                    foreach (var item in itemList)
                    {
                        Presentator.WriteLine($"-------------------------------------");
                        var itemProperties = item.GetType().GetProperties();
                        foreach(var itemProperty in itemProperties)
                        {
                            if (!itemProperty.PropertyType.IsClass || itemProperty.PropertyType == typeof(string))
                            {
                                Presentator.WriteLine($"    - {itemProperty.Name}: {itemProperty.GetValue(item)}");
                            }
                        }
                    }
                    
                }
                else
                {
                    if (!property.PropertyType.IsClass || property.PropertyType == typeof(string))
                    {
                        Presentator.WriteLine($"- {property.Name}: {property.GetValue(entity)}");
                    }
                }


            }
        }
        public virtual void Register()
        {
            var entity = new T();
            Presentator.WriteLine($"Registro de nuevo {typeof(T).Name}");
            var properties = typeof(T).GetProperties();
            foreach (var property in properties) {
                if (ShouldSkipPropertie(property)) continue;
                Presentator.WriteLine($"Ingrese {property.Name}: ");
                string input = Console.ReadLine();
                try {
                    var convertValue = Convert.ChangeType(input, property.PropertyType);
                    property.SetValue(entity, convertValue);
                }catch (Exception ex) { Presentator.WriteLine(ex.Message); }
            }
            _repository.Add(entity);
        }
        public void Update(int id)
        {
            
            var entity = _repository.GetByID(id);
            Presentator.WriteLine($"Modificar un {typeof(T).Name}");
            // Obtenemos las propiedades de la entidad a la que le haremos un update
            var properties = typeof(T).GetProperties();
            Presentator.WriteLine("Que desea modificar?");
            // Comenzamos con un contador de 1 (Por temas visuales ) al momento de mostrar las propiedades cambiables
            int count = 1;
            // Lista de propiedades
            List<PropertyInfo> modifiablePropertys= new List<PropertyInfo>();
            // Recorremos y añadimos a la lista de propiedades, skipeando las inmodificables
            foreach (var property in properties) {
                if (ShouldSkipPropertie(property)) continue;
                Presentator.WriteLine($"{count}) {property.Name}");
                modifiablePropertys.Add(property);
                count++;
            }
            // Pedimos la opcion
            int option = int.Parse(Console.ReadLine());
            // Leemos el nuevo valor
            Presentator.WriteLine("Ingrese el nuevo valor");
            string input = Console.ReadLine();
            // Buscamos la propiedad segun la opcion seleccionada (- 1) -> porque al mostrar la lista comenzamos con el 1
            // Por tanto si no restamos devolvera el siguiente valor al que en realidad pide el usuario
            var changeProperty =  modifiablePropertys.ElementAt(option - 1);
            // Cambiamos los tipos para que coincidan con los pedidos por la propiedad en cuestion
            var convertedValue = Convert.ChangeType(input, changeProperty.PropertyType);
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
