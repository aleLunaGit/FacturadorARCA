using Parcial3.Interfaces;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Modules
{
    public class CrudService<T> where T : class, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<T> _repository;
        public CrudService(IRepository<T> entity)
        {
            _repository = entity;
        }
        public virtual void Register()
        {
            var entity = new T();
            Console.WriteLine($"Registro de nuevo {typeof(T).Name}");
            var properties = typeof(T).GetProperties();
            foreach (var property in properties) {
                if (ShouldSkipPropertie(property)) continue;
                Console.WriteLine($"Ingrese {property.Name}: ");
                string input = Console.ReadLine();
                try {
                    var convertValue = Convert.ChangeType(input, property.PropertyType);
                    property.SetValue(entity, convertValue);
                }catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            _repository.Add(entity);
        }
        public void Update(int id)
        {
            
            var entity = _repository.GetByID(id);
            Console.WriteLine($"Modificar un {typeof(T).Name}");
            // Obtenemos las propiedades de la entidad a la que le haremos un update
            var properties = typeof(T).GetProperties();
            Console.WriteLine("Que desea modificar?");
            // Comenzamos con un contador de 1 (Por temas visuales ) al momento de mostrar las propiedades cambiables
            int count = 1;
            // Lista de propiedades
            List<PropertyInfo> modifiablePropertys= new List<PropertyInfo>();
            // Recorremos y añadimos a la lista de propiedades, skipeando las inmodificables
            foreach (var property in properties) {
                if (ShouldSkipPropertie(property)) continue;
                Console.WriteLine($"{count}) {property.Name}");
                modifiablePropertys.Add(property);
                count++;
            }
            // Pedimos la opcion
            int option = int.Parse(Console.ReadLine());
            // Leemos el nuevo valor
            Console.WriteLine("Ingrese el nuevo valor");
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
        public bool ShouldSkipPropertie(PropertyInfo property)
        {
            // Ignora la propiedad "Id" porque es generada por la base de datos
            if (property.Name == "Id") return true;

            // Ignora propiedades que son listas (relaciones de uno a muchos)
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>)) return true;

            // Ignora propiedades que son otras clases (relaciones de uno a uno)
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string)) return true;

            // Ignora propiedades que no queremos que el usuario ingrese manualmente
            if (property.Name == "ClientId" || property.Name == "InvoiceId") return true;


            return false;
        }
    }
}
